
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UniEnroll.Application.Abstractions;

namespace UniEnroll.Infrastructure.EF.Security
{
    /// <summary>
    /// Simple ADO.NET implementation backed by Users, UserRoles, and RolePermissions tables.
    /// </summary>
    public sealed class EfPermissionService : IPermissionService
    {
        private readonly string _cs;

        public EfPermissionService(IConfiguration config)
        {
            _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? string.Empty;
        }

        public async Task<bool> HasPermissionAsync(string tenantId, string userId, string permission, CancellationToken ct = default)
        {
            const string sql = @"
                                SELECT TOP (1) 1
                                FROM Users u WITH (NOLOCK)
                                JOIN UserRoles ur ON ur.UserId = u.Id
                                JOIN RolePermissions rp ON rp.RoleId = ur.RoleId
                                WHERE u.Id = @user AND u.TenantId = @tenant AND rp.Permission = @perm;";

            await using var conn = new SqlConnection(_cs);
            await conn.OpenAsync(ct);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@tenant", SqlDbType.NVarChar, 64){ Value = tenantId });
            cmd.Parameters.Add(new SqlParameter("@user",   SqlDbType.NVarChar, 64){ Value = userId });
            cmd.Parameters.Add(new SqlParameter("@perm",   SqlDbType.NVarChar, 64){ Value = permission });
            var result = await cmd.ExecuteScalarAsync(ct);
            return result is not null;
        }
    }
}
