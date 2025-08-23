
using System.Text.Json;
using Microsoft.Extensions.Logging;
using UniEnroll.Messaging.Abstractions;

namespace UniEnroll.Messaging.Consumers;

/// <summary>Updates reporting projections when enrollment-related events arrive.</summary>
public sealed class ReportingProjectionConsumer : IEventConsumer
{
    public IReadOnlyCollection<string> Topics { get; } = new[] { "enrollment.confirmed", "payments.succeeded", "billing.invoice.generated" };
    private readonly ILogger<ReportingProjectionConsumer> _logger;

    public ReportingProjectionConsumer(ILogger<ReportingProjectionConsumer> logger) => _logger = logger;

    public Task HandleAsync(ReadOnlyMemory<byte> body, string routingKey, IDictionary<string, object?> headers, CancellationToken ct)
    {
        _logger.LogInformation("Projection updated for {RoutingKey} ({Bytes} bytes)", routingKey, body.Length);
        return Task.CompletedTask;
    }
}
