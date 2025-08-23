
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Serilog;
using UniEnroll.Observability.Logging;
using UniEnroll.Observability.Tracing;
using UniEnroll.Observability.Metrics;
using UniEnroll.Observability.HealthChecks;

namespace UniEnroll.Observability;

public static class DependencyInjection
{
    /// <summary>
    /// Registers logging (Serilog), tracing (OpenTelemetry), metrics, and health checks.
    /// If a hostBuilder is provided, Serilog is wired via UseSerilog().
    /// </summary>
    public static IServiceCollection AddObservability(this IServiceCollection services, IConfiguration config, IHostBuilder? hostBuilder = null)
    {
        // Logging
        if (hostBuilder is not null)
        {
            hostBuilder.UseSerilog((ctx, sp, cfg) =>
            {
                SerilogConfigurator.Configure(cfg, sp, ctx.Configuration);
            });
        }
        else
        {
            // For library-only usage, create a fallback logger to Console.
            Log.Logger = SerilogConfigurator.CreateBootstrapLogger();
        }

        // Tracing & Metrics
        services.AddOpenTelemetryTracing(config);
        services.AddPrometheusMetrics(config);

        // Health checks
        services.AddHealthChecks()
            .AddCheck<SqlServerHealthCheck>("sqlserver", failureStatus: HealthStatus.Unhealthy)
            .AddCheck<RedisHealthCheck>("redis", failureStatus: HealthStatus.Degraded)
            .AddCheck<RabbitMqHealthCheck>("rabbitmq", failureStatus: HealthStatus.Degraded);

        return services;
    }
}
