
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UniEnroll.Infrastructure.Common.Security;

namespace UniEnroll.Seeder.Seeders;

internal sealed class RolesPermissionsSeeder : SeederBase
{
    public RolesPermissionsSeeder(ILogger<RolesPermissionsSeeder> logger, IConfiguration config) : base(logger, config) { }

    public override async Task SeedAsync(CancellationToken ct)
    {
        // Ensure roles table exists and seed default roles
        const string createRoles = @"
IF OBJECT_ID('Roles', 'U') IS NULL
BEGIN
    CREATE TABLE Roles (Id NVARCHAR(64) NOT NULL PRIMARY KEY, Name NVARCHAR(64) NOT NULL UNIQUE);
END";
        await ExecAsync(createRoles, Array.Empty<SqlParameter>(), ct);

        foreach (var role in new[] { "Admin", "Registrar", "Instructor", "Student", "Reporter" })
        {
            var upsert = @"
IF NOT EXISTS (SELECT 1 FROM Roles WHERE Id = @id)
BEGIN
    INSERT INTO Roles (Id, Name) VALUES (@id, @name);
END";
            await ExecAsync(upsert, new[] { P("@id", role.ToLowerInvariant(), SqlDbType.NVarChar, 64), P("@name", role, SqlDbType.NVarChar, 64) }, ct);
        }

        // RolePermissions mapping (create if missing)
        const string ensureRp = @"
IF OBJECT_ID('RolePermissions', 'U') IS NULL
BEGIN
    CREATE TABLE RolePermissions (
        RoleId NVARCHAR(64) NOT NULL,
        Permission NVARCHAR(64) NOT NULL,
        CONSTRAINT PK_RolePermissions PRIMARY KEY (RoleId, Permission)
    );
END";
        await ExecAsync(ensureRp, Array.Empty<SqlParameter>(), ct);

        // Seed a minimal mapping: Admin gets all; others get scoped sets
        foreach (var perm in PermissionRegistry.All)
        {
            await ExecAsync("IF NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId='admin' AND Permission=@p) INSERT INTO RolePermissions(RoleId,Permission) VALUES('admin',@p);",
                new[] { P("@p", perm, SqlDbType.NVarChar, 64) }, ct);
        }

        foreach (var perm in new[] { "registrar.read", "instructor.write", "report.read" })
            await ExecAsync("IF NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId='registrar' AND Permission=@p) INSERT INTO RolePermissions(RoleId,Permission) VALUES('registrar',@p);",
                new[] { P("@p", perm, SqlDbType.NVarChar, 64) }, ct);

        foreach (var perm in new[] { "instructor.read", "instructor.write" })
            await ExecAsync("IF NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId='instructor' AND Permission=@p) INSERT INTO RolePermissions(RoleId,Permission) VALUES('instructor',@p);",
                new[] { P("@p", perm, SqlDbType.NVarChar, 64) }, ct);

        foreach (var perm in new[] { "student.read" })
            await ExecAsync("IF NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId='student' AND Permission=@p) INSERT INTO RolePermissions(RoleId,Permission) VALUES('student',@p);",
                new[] { P("@p", perm, SqlDbType.NVarChar, 64) }, ct);

        foreach (var perm in new[] { "report.read" })
            await ExecAsync("IF NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId='reporter' AND Permission=@p) INSERT INTO RolePermissions(RoleId,Permission) VALUES('reporter',@p);",
                new[] { P("@p", perm, SqlDbType.NVarChar, 64) }, ct);

        _logger.LogInformation("RolesPermissionsSeeder executed.");
    }
}
