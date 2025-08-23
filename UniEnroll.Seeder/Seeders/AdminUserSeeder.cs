
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UniEnroll.Seeder.Seeders;

internal abstract class SeederBase : UniEnroll.Seeder.IDataSeeder
{
    protected readonly ILogger _logger;
    protected readonly string _cs;

    protected SeederBase(ILogger logger, IConfiguration config)
    {
        _logger = logger;
        _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? string.Empty;
    }

    public abstract Task SeedAsync(CancellationToken ct);

    protected async Task<int> ExecAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken ct)
    {
        await using var conn = new SqlConnection(_cs);
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        foreach (var p in parameters) cmd.Parameters.Add(p);
        return await cmd.ExecuteNonQueryAsync(ct);
    }

    protected static SqlParameter P(string name, object? value, SqlDbType type, int? size = null)
    {
        var p = new SqlParameter(name, type) { Value = value ?? DBNull.Value };
        if (size.HasValue) p.Size = size.Value;
        return p;
    }
}

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
        TenantId NVARCHAR(64) NOT NULL
    );
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

        const string upsert = @"
IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = @email)
BEGIN
    INSERT INTO Users (Id, Email, FirstName, LastName, TenantId) VALUES ('admin', @email, @first, @last, @tenant);
END";
        await ExecAsync(upsert, new[] {
            P("@email", email, SqlDbType.NVarChar, 256),
            P("@first", first, SqlDbType.NVarChar, 64),
            P("@last", last, SqlDbType.NVarChar, 64),
            P("@tenant", tid, SqlDbType.NVarChar, 64)
        }, ct);

        const string addRole = @"
IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE UserId='admin' AND RoleId='admin')
BEGIN
    INSERT INTO UserRoles (UserId, RoleId) VALUES ('admin','admin');
END";
        await ExecAsync(addRole, Array.Empty<SqlParameter>(), ct);

        _logger.LogInformation("AdminUserSeeder executed.");
    }
}
