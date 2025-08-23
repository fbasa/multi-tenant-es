
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UniEnroll.Seeder.Seeders;

internal sealed class TenantSeeder : SeederBase
{
    private readonly IConfiguration _config;

    public TenantSeeder(ILogger<TenantSeeder> logger, IConfiguration config) : base(logger, config)
    { _config = config; }

    public override async Task SeedAsync(CancellationToken ct)
    {
        var tid = _config["Seeder:Tenant:Id"] ?? "default";
        var name = _config["Seeder:Tenant:Name"] ?? "Demo University PH";

        const string sql = @"
IF NOT EXISTS (SELECT 1 FROM Tenants WHERE Id = @id)
BEGIN
    INSERT INTO Tenants (Id, Name) VALUES (@id, @name);
END";
        var rows = await ExecAsync(sql, new[] { P("@id", tid, SqlDbType.NVarChar, 64), P("@name", name, SqlDbType.NVarChar, 128) }, ct);
        _logger.LogInformation("TenantSeeder executed. Rows affected: {Rows}", rows);
    }
}
