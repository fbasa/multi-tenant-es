using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using UniEnroll.Infrastructure.Common.Security;

namespace UniEnroll.Observability.Logging;

public static class SerilogConfigurator
{
    public static void Configure(LoggerConfiguration cfg, IServiceProvider services, IConfiguration config)
    {
        var min = config["Serilog:MinimumLevel"] ?? "Information";
        var level = Enum.TryParse(min, ignoreCase: true, out LogEventLevel parsed) ? parsed : LogEventLevel.Information;

        cfg.MinimumLevel.Is(level)
           .Enrich.FromLogContext()
           .Enrich.With(new CorrelationTraceEnricher())
           .WriteTo.Console(new JsonFormatter(renderMessage: true));

        // Optional PII redaction through a custom filter/enricher.
        var redactor = services.GetService<PiiRedactor>();
        if (redactor is not null)
        {
            cfg.Enrich.With(new PiiRedactionEnricher(redactor));
        }
    }

    /// <summary>Bootstrap logger for early startup logs before DI is ready.</summary>
    public static ILogger CreateBootstrapLogger()
        => new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.With(new CorrelationTraceEnricher())
            .WriteTo.Console(new JsonFormatter(renderMessage: true))
            .CreateLogger();
}
