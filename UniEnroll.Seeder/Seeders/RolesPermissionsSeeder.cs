
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UniEnroll.Infrastructure.Common.Security;

namespace UniEnroll.Seeder.Seeders;

internal sealed class RolesPermissionsSeeder : SeederBase
{
    private readonly string _cs;
    private readonly ILogger<RolesPermissionsSeeder> _log;

    public RolesPermissionsSeeder(IConfiguration config, ILogger<RolesPermissionsSeeder> log) : base(log,config)
    {
        _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? throw new InvalidOperationException("Sql connection missing");
        _log = log;
    }

    public override async Task SeedAsync(CancellationToken ct)
    {
        // Ensure tables
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);

        var ensureSql = @"
IF OBJECT_ID('Roles','U') IS NULL
BEGIN
  CREATE TABLE Roles(
    Id NVARCHAR(64) NOT NULL PRIMARY KEY,
    Name NVARCHAR(128) NOT NULL UNIQUE
  );
END
IF OBJECT_ID('RolePermissions','U') IS NULL
BEGIN
  CREATE TABLE RolePermissions(
    RoleId NVARCHAR(64) NOT NULL,
    Permission NVARCHAR(128) NOT NULL,
    CONSTRAINT PK_RolePermissions PRIMARY KEY(RoleId, Permission)
  );
END";
        await using (var ensure = new SqlCommand(ensureSql, conn))
        { await ensure.ExecuteNonQueryAsync(ct); }

        // Role → permissions mapping
        var adminPerms = PermissionRegistry.All;

        var registrarPerms = PermissionRegistry.Groups.RegistrarManage
            .Concat(PermissionRegistry.Groups.RegistrarView)
            .Concat(PermissionRegistry.Groups.SchedulingBuild)
            .Concat(PermissionRegistry.Groups.SchedulingAssignRoom)
            .Concat(PermissionRegistry.Groups.SchedulingOptimize)
            .Concat(PermissionRegistry.Groups.SchedulingViewConflicts)
            .Concat(PermissionRegistry.Groups.SchedulingViewStudentSchedule)
            .Concat(PermissionRegistry.Groups.BillingView) // registrar can view, but not capture/refund
            .ToArray();

        var bursarPerms = PermissionRegistry.Groups.BillingView
            .Concat(PermissionRegistry.Groups.BillingCapture)
            .Concat(PermissionRegistry.Groups.BillingRefund)
            .Concat(PermissionRegistry.Groups.ReportingView) // typical finance need
            .ToArray();

        var instructorPerms = PermissionRegistry.Groups.InstructorView
            .Concat(PermissionRegistry.Groups.InstructorManage)
            .Concat(PermissionRegistry.Groups.InstructorAssign)
            .ToArray();

        var reportingPerms = PermissionRegistry.Groups.ReportingView
            .Concat(PermissionRegistry.Groups.ReportingExport)
            .ToArray();

        var studentPerms = PermissionRegistry.Groups.StudentEnroll
            .Concat(PermissionRegistry.Groups.StudentPortal)
            .Concat(PermissionRegistry.Groups.StudentViewTranscript)
            .ToArray();

        var roleMap = new Dictionary<string, string[]>
        {
            ["admin"]     = adminPerms,
            ["registrar"] = registrarPerms,
            ["bursar"]    = bursarPerms,
            ["instructor"]= instructorPerms,
            ["reporter"]  = reportingPerms,
            ["student"]   = studentPerms
        };

        // Upsert roles
        foreach (var role in roleMap.Keys)
        {
            await UpsertRoleAsync(conn, role, role, ct);
        }

        // Upsert role-permissions
        foreach (var kvp in roleMap)
        {
            foreach (var perm in kvp.Value.Distinct())
            {
                if (!PermissionRegistry.IsKnown(perm))
                {
                    _log.LogWarning("Unknown permission '{Perm}' referenced by role '{Role}'", perm, kvp.Key);
                    continue;
                }
                await UpsertRolePermissionAsync(conn, kvp.Key, perm, ct);
            }
        }

        _log.LogInformation("Roles & permissions seeding completed: {Count} roles", roleMap.Count);
    }

    private static async Task UpsertRoleAsync(SqlConnection conn, string id, string name, CancellationToken ct)
    {
        await using var cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM Roles WHERE Id=@id) UPDATE Roles SET Name=@name WHERE Id=@id
ELSE INSERT INTO Roles (Id, Name) VALUES (@id, @name)", conn);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, 64){ Value = id });
        cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 128){ Value = name });
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static async Task UpsertRolePermissionAsync(SqlConnection conn, string roleId, string permission, CancellationToken ct)
    {
        await using var cmd = new SqlCommand(@"
IF NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId=@r AND Permission=@p)
    INSERT INTO RolePermissions (RoleId, Permission) VALUES (@r, @p)", conn);
        cmd.Parameters.Add(new SqlParameter("@r", SqlDbType.NVarChar, 64){ Value = roleId });
        cmd.Parameters.Add(new SqlParameter("@p", SqlDbType.NVarChar, 128){ Value = permission });
        await cmd.ExecuteNonQueryAsync(ct);
    }
}
