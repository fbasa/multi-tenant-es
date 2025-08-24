using System.Diagnostics;
using Serilog.Events;
using Serilog.Core;
using UniEnroll.Infrastructure.Common.Logging;

namespace UniEnroll.Observability.Logging;

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
