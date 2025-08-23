
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UniEnroll.Observability.HealthChecks;

/// <summary>
/// Verifies DB connectivity and that READ_COMMITTED_SNAPSHOT is ON for the target database.
/// </summary>
public sealed class SqlServerHealthCheck : IHealthCheck
{
    private readonly string _cs;
    public SqlServerHealthCheck(IConfiguration config)
        => _cs = config.GetConnectionString("Sql") ?? config["Sql:ConnectionString"] ?? string.Empty;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_cs))
            return HealthCheckResult.Unhealthy("Missing SQL connection string");

        try
        {
            await using var conn = new SqlConnection(_cs);
            await conn.OpenAsync(cancellationToken);

            // Check RCSI
            await using var cmd = new SqlCommand("SELECT is_read_committed_snapshot_on FROM sys.databases WHERE name = DB_NAME();", conn);
            var val = (int)(await cmd.ExecuteScalarAsync(cancellationToken) ?? 0);
            var rcsiOn = val == 1;
            return rcsiOn
                ? HealthCheckResult.Healthy("SQL OK; RCSI=ON")
                : HealthCheckResult.Unhealthy("SQL OK; RCSI is OFF");
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }
}
