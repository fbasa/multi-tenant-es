
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using UniEnroll.Infrastructure.Common.Abstractions;
using UniEnroll.Infrastructure.Common.Options;
using UniEnroll.Infrastructure.Common.Security;
using UniEnroll.Infrastructure.EF.Sql;

namespace UniEnroll.Infrastructure.EF.Security;

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

        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);

        await using (var ensure = new SqlCommand(RefreshTokenSql.EnsureTable, conn))
        { await ensure.ExecuteNonQueryAsync(ct); }

        await using (var cmd = new SqlCommand(RefreshTokenSql.Insert, conn))
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

        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);

        Guid id;
        string tenantId;
        string userId;
        string? dbDevice;
        DateTimeOffset expiresAt;
        DateTimeOffset? revokedAt;
        Guid? replacedBy;

        await using (var get = new SqlCommand(RefreshTokenSql.SelectByHash, conn))
        {
            get.Parameters.Add(new SqlParameter("@h", SqlDbType.VarBinary, 32){ Value = hash });
            await using var rdr = await get.ExecuteReaderAsync(ct);
            if (!await rdr.ReadAsync(ct))
                return new RefreshRotateResult(false, null, null, Array.Empty<string>(), null, null, "invalid_refresh_token");

            id        = rdr.GetGuid(0);
            tenantId  = rdr.GetString(1);
            userId    = rdr.GetString(2);
            dbDevice  = rdr.IsDBNull(3) ? null : rdr.GetString(3);
            expiresAt = rdr.GetDateTimeOffset(4);
            revokedAt = rdr.IsDBNull(5) ? (DateTimeOffset?)null : rdr.GetDateTimeOffset(5);
            replacedBy= rdr.IsDBNull(6) ? (Guid?)null : rdr.GetGuid(6);
        }

        if (expectedTenantId is not null && !string.Equals(expectedTenantId, tenantId, StringComparison.Ordinal))
            return new RefreshRotateResult(false, null, null, Array.Empty<string>(), null, null, "wrong_tenant");

        if (revokedAt is not null || replacedBy is not null)
        {
            await RevokeChainAsync(conn, id, ip, "reuse_detected", ct);
            return new RefreshRotateResult(false, null, null, Array.Empty<string>(), null, null, "reused_or_revoked");
        }

        if (expiresAt <= DateTimeOffset.UtcNow)
            return new RefreshRotateResult(false, null, null, Array.Empty<string>(), null, null, "expired");

        if (deviceId is not null && !string.Equals(deviceId, dbDevice, StringComparison.Ordinal))
            return new RefreshRotateResult(false, null, null, Array.Empty<string>(), null, null, "wrong_device");

        var issue = await IssueAsync(tenantId, userId, dbDevice ?? deviceId, ip, ct);

        await using (var cmd = new SqlCommand(RefreshTokenSql.RevokeOnRotate, conn))
        {
            cmd.Parameters.Add(new SqlParameter("@now", SqlDbType.DateTimeOffset){ Value = DateTimeOffset.UtcNow });
            cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.NVarChar, 64){ Value = ip });
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier){ Value = id });
            cmd.Parameters.Add(new SqlParameter("@hNew", SqlDbType.VarBinary, 32){ Value = TokenHasher.Sha256(issue.RefreshToken) });
            await cmd.ExecuteNonQueryAsync(ct);
        }

        var roles = new System.Collections.Generic.List<string>();
        await using (var rc = new SqlCommand(IdentitySql.SelectRolesByUser, conn))
        {
            rc.Parameters.Add(new SqlParameter("@userId", SqlDbType.NVarChar, 64){ Value = userId });
            await using var rr = await rc.ExecuteReaderAsync(ct);
            while (await rr.ReadAsync(ct)) roles.Add(rr.GetString(0));
        }

        return new RefreshRotateResult(true, userId, tenantId, roles.ToArray(), issue.RefreshToken, issue.ExpiresAt, null);
    }

    public async Task RevokeAsync(string token, string ip, string reason, CancellationToken ct = default)
    {
        var hash = TokenHasher.Sha256(token);
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(RefreshTokenSql.RevokeByHash, conn);
        cmd.Parameters.Add(new SqlParameter("@now", SqlDbType.DateTimeOffset){ Value = DateTimeOffset.UtcNow });
        cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.NVarChar, 64){ Value = ip });
        cmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.NVarChar, 128){ Value = reason });
        cmd.Parameters.Add(new SqlParameter("@h", SqlDbType.VarBinary, 32){ Value = hash });
        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task RevokeAllForUserAsync(string tenantId, string userId, string ip, string reason, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(RefreshTokenSql.RevokeAllForUser, conn);
        cmd.Parameters.Add(new SqlParameter("@now", SqlDbType.DateTimeOffset){ Value = DateTimeOffset.UtcNow });
        cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.NVarChar, 64){ Value = ip });
        cmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.NVarChar, 128){ Value = reason });
        cmd.Parameters.Add(new SqlParameter("@t", SqlDbType.NVarChar, 64){ Value = tenantId });
        cmd.Parameters.Add(new SqlParameter("@u", SqlDbType.NVarChar, 64){ Value = userId });
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static async Task RevokeChainAsync(SqlConnection conn, Guid id, string ip, string reason, CancellationToken ct)
    {
        await using var cmd = new SqlCommand(RefreshTokenSql.RevokeChain, conn);
        cmd.Parameters.Add(new SqlParameter("@now", SqlDbType.DateTimeOffset){ Value = DateTimeOffset.UtcNow });
        cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.NVarChar, 64){ Value = ip });
        cmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.NVarChar, 128){ Value = reason });
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier){ Value = id });
        await cmd.ExecuteNonQueryAsync(ct);
    }
}
