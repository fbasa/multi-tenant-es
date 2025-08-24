using Serilog.Events;
using Serilog.Core;
using UniEnroll.Infrastructure.Common.Security;

namespace UniEnroll.Observability.Logging;

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
