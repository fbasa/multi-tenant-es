using System;
using System.Data;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UniEnroll.Seeder.Seeders;

internal sealed class AdminUserSeeder : SeederBase
{
    private readonly string _cs;
    private readonly ILogger<AdminUserSeeder> _log;
    private readonly IConfiguration _config;

    public AdminUserSeeder(IConfiguration config, ILogger<AdminUserSeeder> log) : base(log,config)
    {
        _config = config;
        _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? throw new InvalidOperationException("Sql connection missing");
        _log = log;
    }

    public async override Task SeedAsync(CancellationToken ct = default)
    {
        var tenantId = _config["Admin:TenantId"] ?? "root";
        var userId   = _config["Admin:UserId"]   ?? "admin";
        var email    = _config["Admin:Email"]    ?? "admin@university.local";
        var pwd      = _config["Admin:Password"] ?? "ChangeMe!123";

        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);

        await EnsureTablesAsync(conn, ct);
        await UpsertAdminRoleAsync(conn, ct);
        await UpsertAdminUserAsync(conn, tenantId, userId, email, pwd, ct);
        await UpsertUserRoleAsync(conn, userId, "admin", ct);

        _log.LogInformation("Admin user '{UserId}' ensured for tenant '{TenantId}' with role 'admin'", userId, tenantId);
    }

    private static async Task EnsureTablesAsync(SqlConnection conn, CancellationToken ct)
    {
        var sql = @"
IF OBJECT_ID('Users','U') IS NULL
BEGIN
  CREATE TABLE Users(
    Id NVARCHAR(64) NOT NULL PRIMARY KEY,
    TenantId NVARCHAR(64) NOT NULL,
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash VARBINARY(64) NULL,
    PasswordSalt VARBINARY(16) NULL,
    PasswordAlgo NVARCHAR(64) NULL,
    CreatedAt DATETIMEOFFSET(7) NOT NULL DEFAULT SYSUTCDATETIME()
  );
  CREATE INDEX IX_Users_Tenant_Email ON Users(TenantId, Email);
END
IF OBJECT_ID('UserRoles','U') IS NULL
BEGIN
  CREATE TABLE UserRoles(
    UserId NVARCHAR(64) NOT NULL,
    RoleId NVARCHAR(64) NOT NULL,
    CONSTRAINT PK_UserRoles PRIMARY KEY(UserId, RoleId)
  );
END
IF OBJECT_ID('Roles','U') IS NULL
BEGIN
  CREATE TABLE Roles(
    Id NVARCHAR(64) NOT NULL PRIMARY KEY,
    Name NVARCHAR(128) NOT NULL UNIQUE
  );
END";
        await using var cmd = new SqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static async Task UpsertAdminRoleAsync(SqlConnection conn, CancellationToken ct)
    {
        await using var cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM Roles WHERE Id='admin') UPDATE Roles SET Name='Administrator' WHERE Id='admin'
ELSE INSERT INTO Roles (Id, Name) VALUES ('admin','Administrator')", conn);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static async Task UpsertAdminUserAsync(SqlConnection conn, string tenantId, string userId, string email, string password, CancellationToken ct)
    {
        // PBKDF2-SHA256 with 100k iterations
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = PBKDF2(password, salt, 100_000, 32);

        await using var cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM Users WHERE Id=@id)
BEGIN
  UPDATE Users SET TenantId=@tenant, Email=@email, PasswordHash=@hash, PasswordSalt=@salt, PasswordAlgo='PBKDF2-SHA256-100000' WHERE Id=@id;
END
ELSE
BEGIN
  INSERT INTO Users (Id, TenantId, Email, PasswordHash, PasswordSalt, PasswordAlgo)
  VALUES (@id, @tenant, @email, @hash, @salt, 'PBKDF2-SHA256-100000');
END", conn);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, 64){ Value = userId });
        cmd.Parameters.Add(new SqlParameter("@tenant", SqlDbType.NVarChar, 64){ Value = tenantId });
        cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 256){ Value = email });
        cmd.Parameters.Add(new SqlParameter("@hash", SqlDbType.VarBinary, 64){ Value = hash });
        cmd.Parameters.Add(new SqlParameter("@salt", SqlDbType.VarBinary, 16){ Value = salt });
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static async Task UpsertUserRoleAsync(SqlConnection conn, string userId, string roleId, CancellationToken ct)
    {
        await using var cmd = new SqlCommand(@"
IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE UserId=@u AND RoleId=@r)
    INSERT INTO UserRoles (UserId, RoleId) VALUES (@u, @r)", conn);
        cmd.Parameters.Add(new SqlParameter("@u", SqlDbType.NVarChar, 64){ Value = userId });
        cmd.Parameters.Add(new SqlParameter("@r", SqlDbType.NVarChar, 64){ Value = roleId });
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static byte[] PBKDF2(string password, byte[] salt, int iter, int len)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iter, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(len);
    }
}