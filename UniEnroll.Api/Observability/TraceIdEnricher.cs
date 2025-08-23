
using Serilog.Core;
using Serilog.Events;

namespace UniEnroll.Api.Observability;

public sealed class TraceIdEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _http;
    public TraceIdEnricher(IHttpContextAccessor http) => _http = http;
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var corr = _http.HttpContext?.Items.TryGetValue("X-Correlation-Id", out var v) == true ? v?.ToString() : null;
        if (!string.IsNullOrWhiteSpace(corr))
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("correlationId", corr!));
        }
    }
}
