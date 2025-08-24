
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using UniEnroll.Infrastructure.Common.Security;

namespace UniEnroll.Seeder.Seeders;

internal sealed class AdminUserSeeder : SeederBase
{
    private readonly IConfiguration _config;
    public AdminUserSeeder(ILogger<AdminUserSeeder> logger, IConfiguration config) : base(logger, config) { _config = config; }

    public override async Task SeedAsync(CancellationToken ct)
    {
        const string ensureUsers = @"
IF OBJECT_ID('Users','U') IS NULL
BEGIN
    CREATE TABLE Users (
        Id NVARCHAR(64) NOT NULL PRIMARY KEY,
        Email NVARCHAR(256) NOT NULL UNIQUE,
        FirstName NVARCHAR(64) NULL,
        LastName NVARCHAR(64) NULL,
        TenantId NVARCHAR(64) NOT NULL,
        PasswordHash VARBINARY(64) NULL,
        PasswordSalt VARBINARY(16) NULL
    );
END
ELSE
BEGIN
    IF COL_LENGTH('Users','PasswordHash') IS NULL ALTER TABLE Users ADD PasswordHash VARBINARY(64) NULL;
    IF COL_LENGTH('Users','PasswordSalt') IS NULL ALTER TABLE Users ADD PasswordSalt VARBINARY(16) NULL;
END
IF OBJECT_ID('UserRoles','U') IS NULL
BEGIN
    CREATE TABLE UserRoles (
        UserId NVARCHAR(64) NOT NULL,
        RoleId NVARCHAR(64) NOT NULL,
        CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId)
    );
END";
        await ExecAsync(ensureUsers, Array.Empty<SqlParameter>(), ct);

        var email = _config["Seeder:Admin:Email"] ?? "admin@demo.edu.ph";
        var tid = _config["Seeder:Tenant:Id"] ?? "default";
        var first = _config["Seeder:Admin:FirstName"] ?? "System";
        var last = _config["Seeder:Admin:LastName"] ?? "Administrator";
        var password = _config["Seeder:Admin:Password"] ?? "ChangeMe!123";

        var (hash, salt) = PasswordHasher.Hash(password);

        const string upsert = @"
IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = 'admin')
    INSERT INTO Users (Id, Email, FirstName, LastName, TenantId, PasswordHash, PasswordSalt)
    VALUES ('admin', @email, @first, @last, @tenant, @hash, @salt);
ELSE
    UPDATE Users SET Email=@email, FirstName=@first, LastName=@last, TenantId=@tenant, PasswordHash=@hash, PasswordSalt=@salt WHERE Id='admin';";
        await ExecAsync(upsert, new[] {
            P("@email", email, SqlDbType.NVarChar, 256),
            P("@first", first, SqlDbType.NVarChar, 64),
            P("@last", last, SqlDbType.NVarChar, 64),
            P("@tenant", tid, SqlDbType.NVarChar, 64),
            new SqlParameter("@hash", SqlDbType.VarBinary, 64){ Value = hash },
            new SqlParameter("@salt", SqlDbType.VarBinary, 16){ Value = salt }
        }, ct);

        const string addRole = @"
IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE UserId='admin' AND RoleId='admin')
BEGIN
    INSERT INTO UserRoles (UserId, RoleId) VALUES ('admin','admin');
END";
        await ExecAsync(addRole, Array.Empty<SqlParameter>(), ct);

        _logger.LogInformation("AdminUserSeeder executed (with password hash).");
    }
}

