
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using UniEnroll.Infrastructure.Common.Tenancy;

namespace UniEnroll.Observability.Logging;

public sealed class TraceIdEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _http;
    public TraceIdEnricher(IHttpContextAccessor http) => _http = http;
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var corr = _http.HttpContext?.Items.TryGetValue(TenantHeaderNames.CorrelationId, out var v) == true ? v?.ToString() : null;
        if (!string.IsNullOrWhiteSpace(corr))
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("correlationId", corr!));
        }
    }
}
