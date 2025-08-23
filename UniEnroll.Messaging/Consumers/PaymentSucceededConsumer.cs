
using System.Text.Json;
using Microsoft.Extensions.Logging;
using UniEnroll.Contracts.Events;
using UniEnroll.Messaging.Abstractions;

namespace UniEnroll.Messaging.Consumers;

public sealed class PaymentSucceededConsumer : IEventConsumer
{
    public IReadOnlyCollection<string> Topics { get; } = new[] { "payments.succeeded" };
    private readonly ILogger<PaymentSucceededConsumer> _logger;

    public PaymentSucceededConsumer(ILogger<PaymentSucceededConsumer> logger) => _logger = logger;

    public Task HandleAsync(ReadOnlyMemory<byte> body, string routingKey, IDictionary<string, object?> headers, CancellationToken ct)
    {
        var evt = JsonSerializer.Deserialize<PaymentSucceededV1>(body.Span);
        _logger.LogInformation("Payment succeeded: {PaymentId} Invoice={InvoiceId} Amount={Amount}", evt?.PaymentId, evt?.InvoiceId, evt?.Amount.Amount);
        return Task.CompletedTask;
    }
}
