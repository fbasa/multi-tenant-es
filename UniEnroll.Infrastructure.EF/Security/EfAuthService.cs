
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UniEnroll.Application.Abstractions;
using UniEnroll.Infrastructure.Common.Security;

namespace UniEnroll.Infrastructure.EF.Security;

public sealed class EfAuthService : IAuthService
{
    private readonly string _cs;
    public EfAuthService(IConfiguration config)
        => _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? string.Empty;

    public async Task<AuthResult?> AuthenticateAsync(string tenantId, string email, string password, CancellationToken ct = default)
    {
        const string sql = @"
SELECT TOP (1) u.Id, u.Email, u.TenantId, u.PasswordHash, u.PasswordSalt
FROM Users u
WHERE u.Email = @email AND u.TenantId = @tenant;";

        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 256){ Value = email });
        cmd.Parameters.Add(new SqlParameter("@tenant", SqlDbType.NVarChar, 64){ Value = tenantId });

        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        if (!await rdr.ReadAsync(ct)) return null;

        var userId = rdr.GetString(0);
        var userEmail = rdr.GetString(1);
        var userTenant = rdr.GetString(2);
        var hash = (byte[])rdr["PasswordHash"];
        var salt = (byte[])rdr["PasswordSalt"];

        if (!PasswordHasher.Verify(password, hash, salt))
            return null;

        // Load roles
        const string rolesSql = @"SELECT ur.RoleId FROM UserRoles ur WHERE ur.UserId = @userId";
        var roles = new List<string>();
        await using (var cmdRoles = new SqlCommand(rolesSql, conn))
        {
            cmdRoles.Parameters.Add(new SqlParameter("@userId", SqlDbType.NVarChar, 64){ Value = userId });
            await using var rr = await cmdRoles.ExecuteReaderAsync(ct);
            while (await rr.ReadAsync(ct)) roles.Add(rr.GetString(0));
        }

        return new AuthResult(userId, userEmail, userTenant, roles.ToArray());
    }
}
