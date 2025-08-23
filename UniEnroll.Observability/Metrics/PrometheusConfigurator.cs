
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;

namespace UniEnroll.Observability.Metrics;

public static class PrometheusConfigurator
{
    public static IServiceCollection AddPrometheusMetrics(this IServiceCollection services, IConfiguration config)
    {
        services.AddOpenTelemetry()
            .WithMetrics(b =>
            {
                b.AddAspNetCoreInstrumentation()
                 .AddHttpClientInstrumentation()
                 .AddRuntimeInstrumentation()
                 .AddProcessInstrumentation()
                 .AddPrometheusExporter();
            });

        return services;
    }

    /// <summary>Map /metrics endpoint for Prometheus to scrape.</summary>
    public static IApplicationBuilder UsePrometheusEndpoint(this IApplicationBuilder app, string path = "/metrics")
    {
        app.UseOpenTelemetryPrometheusScrapingEndpoint(path);
        return app;
    }
}
