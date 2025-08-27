using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using UniEnroll.Infrastructure.EF.Repositories.Contracts;
using UniEnroll.Infrastructure.EF.Sql;

namespace UniEnroll.Infrastructure.EF.Repositories;

public sealed class PermissionRepository : IPermissionRepository
{
    private readonly string _cs;
    public PermissionRepository(IConfiguration config)
        => _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? string.Empty;

    public async Task<bool> HasPermissionAsync(string tenantId, string userId, string permission, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(PermissionSql.HasPermission, conn);
        cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.NVarChar, 64) { Value = userId });
        cmd.Parameters.Add(new SqlParameter("@tenant", SqlDbType.NVarChar, 64) { Value = tenantId });
        cmd.Parameters.Add(new SqlParameter("@perm", SqlDbType.NVarChar, 64) { Value = permission });
        var result = await cmd.ExecuteScalarAsync(ct);
        return result is not null;
    }
}