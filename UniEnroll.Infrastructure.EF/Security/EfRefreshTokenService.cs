
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using UniEnroll.Application.Abstractions;
using UniEnroll.Infrastructure.Common.Options;
using UniEnroll.Infrastructure.Common.Security;

namespace UniEnroll.Infrastructure.EF.Security;

/// <summary>
/// ADO.NET implementation with rotating, revocable refresh tokens (hashed at rest).
/// </summary>
public sealed class EfRefreshTokenService : IRefreshTokenService
{
    private readonly string _cs;
    private readonly RefreshTokenOptions _opts;

    public EfRefreshTokenService(IConfiguration config, IOptions<RefreshTokenOptions> opts)
    {
        _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? string.Empty;
        _opts = opts.Value;
    }

    public async Task<RefreshIssueResult> IssueAsync(string tenantId, string userId, string? deviceId, string createdByIp, CancellationToken ct = default)
    {
        var token = TokenHasher.CreateSecureToken();
        var hash = TokenHasher.Sha256(token);
        var now = DateTimeOffset.UtcNow;
        var expires = now.AddDays(_opts.Days);

        const string ensure = @"
IF OBJECT_ID('RefreshTokens','U') IS NULL
BEGIN
  CREATE TABLE RefreshTokens(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    TenantId NVARCHAR(64) NOT NULL,
    UserId NVARCHAR(64) NOT NULL,
    TokenHash VARBINARY(32) NOT NULL UNIQUE,
    DeviceId NVARCHAR(128) NULL,
    ExpiresAt DATETIMEOFFSET(7) NOT NULL,
    CreatedAt DATETIMEOFFSET(7) NOT NULL,
    CreatedByIp NVARCHAR(64) NULL,
    RevokedAt DATETIMEOFFSET(7) NULL,
    RevokedByIp NVARCHAR(64) NULL,
    ReasonRevoked NVARCHAR(128) NULL,
    ReplacedByTokenId UNIQUEIDENTIFIER NULL
  );
  CREATE INDEX IX_RefreshTokens_User ON RefreshTokens(UserId);
END";

        const string insert = @"
INSERT INTO RefreshTokens (TenantId, UserId, TokenHash, DeviceId, ExpiresAt, CreatedAt, CreatedByIp)
VALUES (@t, @u, @h, @d, @exp, @now, @ip);
";

        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using (var cmdEnsure = new SqlCommand(ensure, conn))
        { await cmdEnsure.ExecuteNonQueryAsync(ct); }

        await using (var cmd = new SqlCommand(insert, conn))
        {
            cmd.Parameters.Add(new SqlParameter("@t", SqlDbType.NVarChar, 64){ Value = tenantId });
            cmd.Parameters.Add(new SqlParameter("@u", SqlDbType.NVarChar, 64){ Value = userId });
            cmd.Parameters.Add(new SqlParameter("@h", SqlDbType.VarBinary, 32){ Value = hash });
            cmd.Parameters.Add(new SqlParameter("@d", SqlDbType.NVarChar, 128){ Value = (object?)deviceId ?? DBNull.Value });
            cmd.Parameters.Add(new SqlParameter("@exp", SqlDbType.DateTimeOffset){ Value = expires });
            cmd.Parameters.Add(new SqlParameter("@now", SqlDbType.DateTimeOffset){ Value = now });
            cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.NVarChar, 64){ Value = (object?)createdByIp ?? DBNull.Value });
            await cmd.ExecuteNonQueryAsync(ct);
        }

        return new RefreshIssueResult(token, expires);
    }

    public async Task<RefreshRotateResult> ValidateAndRotateAsync(string token, string? expectedTenantId, string? deviceId, string ip, CancellationToken ct = default)
    {
        var hash = TokenHasher.Sha256(token);

        const string select = @"
SELECT TOP (1) Id, TenantId, UserId, DeviceId, ExpiresAt, RevokedAt, ReplacedByTokenId
FROM RefreshTokens WHERE TokenHash = @h;";

        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);

        Guid id;
        string tenantId, userId;
        string? dbDevice;
        DateTimeOffset expiresAt;
        DateTimeOffset? revokedAt;
        Guid? replacedBy;
        await using (var get = new SqlCommand(select, conn))
        {
            get.Parameters.Add(new SqlParameter("@h", SqlDbType.VarBinary, 32){ Value = hash });
            await using var rdr = await get.ExecuteReaderAsync(ct);
            if (!await rdr.ReadAsync(ct))
                return new RefreshRotateResult(false, null, null, Array.Empty<string>(), null, null, "invalid_refresh_token");

            id = rdr.GetGuid(0);
            tenantId = rdr.GetString(1);
            userId = rdr.GetString(2);
            dbDevice = rdr.IsDBNull(3) ? null : rdr.GetString(3);
            expiresAt = rdr.GetDateTimeOffset(4);
            revokedAt = rdr.IsDBNull(5) ? (DateTimeOffset?)null : rdr.GetDateTimeOffset(5);
            replacedBy = rdr.IsDBNull(6) ? (Guid?)null : rdr.GetGuid(6);
        }

        if (expectedTenantId is not null && !string.Equals(expectedTenantId, tenantId, StringComparison.Ordinal))
            return new RefreshRotateResult(false, null, null, Array.Empty<string>(), null, null, "wrong_tenant");

        if (revokedAt is not null || replacedBy is not null)
        {
            // Token reuse or already rotated; revoke the whole family for safety
            await RevokeChainAsync(conn, id, ip, "reuse_detected", ct);
            return new RefreshRotateResult(false, null, null, Array.Empty<string>(), null, null, "reused_or_revoked");
        }

        if (expiresAt <= DateTimeOffset.UtcNow)
            return new RefreshRotateResult(false, null, null, Array.Empty<string>(), null, null, "expired");

        if (deviceId is not null && dbDevice is not null && !string.Equals(deviceId, dbDevice, StringComparison.Ordinal))
            return new RefreshRotateResult(false, null, null, Array.Empty<string>(), null, null, "wrong_device");

        // rotate: revoke current and issue a new one
        var issue = await IssueAsync(tenantId, userId, dbDevice ?? deviceId, ip, ct);

        const string revoke = @"
UPDATE RefreshTokens SET RevokedAt=@now, RevokedByIp=@ip, ReasonRevoked='rotated'
WHERE Id=@id;
UPDATE RefreshTokens SET ReplacedByTokenId = (SELECT TOP(1) Id FROM RefreshTokens WHERE TokenHash=@hNew)
WHERE Id=@id;";

        await using (var cmd = new SqlCommand(revoke, conn))
        {
            cmd.Parameters.Add(new SqlParameter("@now", SqlDbType.DateTimeOffset){ Value = DateTimeOffset.UtcNow });
            cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.NVarChar, 64){ Value = ip });
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier){ Value = id });
            cmd.Parameters.Add(new SqlParameter("@hNew", SqlDbType.VarBinary, 32){ Value = TokenHasher.Sha256(issue.RefreshToken) });
            await cmd.ExecuteNonQueryAsync(ct);
        }

        // load roles for new access token issuance
        var roles = await GetRolesAsync(conn, userId, ct);

        return new RefreshRotateResult(true, userId, tenantId, roles, issue.RefreshToken, issue.ExpiresAt, null);
    }

    public async Task RevokeAsync(string token, string ip, string reason, CancellationToken ct = default)
    {
        var hash = TokenHasher.Sha256(token);
        const string sql = "UPDATE RefreshTokens SET RevokedAt=@now, RevokedByIp=@ip, ReasonRevoked=@reason WHERE TokenHash=@h;";
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@now", SqlDbType.DateTimeOffset){ Value = DateTimeOffset.UtcNow });
        cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.NVarChar, 64){ Value = ip });
        cmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.NVarChar, 128){ Value = reason });
        cmd.Parameters.Add(new SqlParameter("@h", SqlDbType.VarBinary, 32){ Value = hash });
        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task RevokeAllForUserAsync(string tenantId, string userId, string ip, string reason, CancellationToken ct = default)
    {
        const string sql = "UPDATE RefreshTokens SET RevokedAt=@now, RevokedByIp=@ip, ReasonRevoked=@reason WHERE TenantId=@t AND UserId=@u AND RevokedAt IS NULL;";
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@now", SqlDbType.DateTimeOffset){ Value = DateTimeOffset.UtcNow });
        cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.NVarChar, 64){ Value = ip });
        cmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.NVarChar, 128){ Value = reason });
        cmd.Parameters.Add(new SqlParameter("@t", SqlDbType.NVarChar, 64){ Value = tenantId });
        cmd.Parameters.Add(new SqlParameter("@u", SqlDbType.NVarChar, 64){ Value = userId });
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static async Task RevokeChainAsync(SqlConnection conn, Guid id, string ip, string reason, CancellationToken ct)
    {
        const string sql = @"
UPDATE RefreshTokens SET RevokedAt=@now, RevokedByIp=@ip, ReasonRevoked=@reason WHERE Id=@id OR ReplacedByTokenId=@id;";
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@now", SqlDbType.DateTimeOffset){ Value = DateTimeOffset.UtcNow });
        cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.NVarChar, 64){ Value = ip });
        cmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.NVarChar, 128){ Value = reason });
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier){ Value = id });
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static async Task<string[]> GetRolesAsync(SqlConnection conn, string userId, CancellationToken ct)
    {
        const string rolesSql = @"SELECT ur.RoleId FROM UserRoles ur WHERE ur.UserId = @userId";
        var roles = new System.Collections.Generic.List<string>();
        await using var cmd = new SqlCommand(rolesSql, conn);
        cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.NVarChar, 64){ Value = userId });
        await using var rr = await cmd.ExecuteReaderAsync(ct);
        while (await rr.ReadAsync(ct)) roles.Add(rr.GetString(0));
        return roles.ToArray();
    }
}
