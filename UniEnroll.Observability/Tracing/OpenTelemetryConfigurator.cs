
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace UniEnroll.Observability.Tracing;

public static class OpenTelemetryConfigurator
{
    public static IServiceCollection AddOpenTelemetryTracing(this IServiceCollection services, IConfiguration config)
    {
        var serviceName = config["Service:Name"] ?? "UniEnroll";
        var serviceVersion = config["Service:Version"] ?? "1.0.0";

        services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(serviceName, serviceVersion: serviceVersion))
            .WithTracing(b =>
            {
                b.AddAspNetCoreInstrumentation()
                 .AddHttpClientInstrumentation()
                 .AddSqlClientInstrumentation(o =>
                 {
                     o.RecordException = true;
                     o.SetDbStatementForText = true;
                 });

                var otlp = config["OpenTelemetry:OtlpEndpoint"];
                if (!string.IsNullOrWhiteSpace(otlp))
                {
                    b.AddOtlpExporter(o => o.Endpoint = new Uri(otlp));
                }
            });

        return services;
    }
}
