
using System.Text.Json;
using Microsoft.Extensions.Logging;
using UniEnroll.Messaging.Abstractions;
using UniEnroll.Contracts.Events;

namespace UniEnroll.Messaging.Consumers;

public sealed class InvoiceGeneratedConsumer : IEventConsumer
{
    public IReadOnlyCollection<string> Topics { get; } = new[] { "billing.invoice.generated" };
    private readonly ILogger<InvoiceGeneratedConsumer> _logger;

    public InvoiceGeneratedConsumer(ILogger<InvoiceGeneratedConsumer> logger) => _logger = logger;

    public Task HandleAsync(ReadOnlyMemory<byte> body, string routingKey, IDictionary<string, object?> headers, CancellationToken ct)
    {
        var evt = JsonSerializer.Deserialize<InvoiceGeneratedV1>(body.Span);
        _logger.LogInformation("Invoice generated: {InvoiceId} Total={Total}", evt?.InvoiceId, evt?.Amount.Amount);
        return Task.CompletedTask;
    }
}
