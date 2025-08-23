
using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Core;
using Serilog.Configuration;
using UniEnroll.Infrastructure.Common.Logging;
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

/// <summary>Enriches logs with correlationId and trace/span IDs.</summary>
internal sealed class CorrelationTraceEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var corr = SafeLogEnricher.GetCorrelationId();
        if (!string.IsNullOrWhiteSpace(corr))
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("correlationId", corr));
        var act = Activity.Current;
        if (act is not null)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("traceId", act.TraceId.ToString()));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("spanId", act.SpanId.ToString()));
        }
    }
}

/// <summary>Simple redaction enricher that masks common PII-like properties.</summary>
internal sealed class PiiRedactionEnricher : ILogEventEnricher
{
    private readonly PiiRedactor _redactor;
    public PiiRedactionEnricher(PiiRedactor redactor) => _redactor = redactor;

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        foreach (var key in new[] { "email", "userEmail", "studentEmail" })
        {
            if (logEvent.Properties.TryGetValue(key, out var val) && val is ScalarValue sv && sv.Value is string s)
            {
                var red = _redactor.RedactEmail(s);
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(key, red));
            }
        }
    }
}
