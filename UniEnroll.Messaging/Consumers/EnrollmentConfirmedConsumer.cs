
using System.Text.Json;
using Microsoft.Extensions.Logging;
using UniEnroll.Messaging.Abstractions;
using UniEnroll.Contracts.Events;

namespace UniEnroll.Messaging.Consumers;

public sealed class EnrollmentConfirmedConsumer : IEventConsumer
{
    public IReadOnlyCollection<string> Topics { get; } = new[] { "enrollment.confirmed" };
    private readonly ILogger<EnrollmentConfirmedConsumer> _logger;

    public EnrollmentConfirmedConsumer(ILogger<EnrollmentConfirmedConsumer> logger) => _logger = logger;

    public Task HandleAsync(ReadOnlyMemory<byte> body, string routingKey, IDictionary<string, object?> headers, CancellationToken ct)
    {
        var evt = JsonSerializer.Deserialize<EnrollmentConfirmedV1>(body.Span);
        _logger.LogInformation("Enrollment confirmed: {EnrollmentId}", evt?.EnrollmentId);
        return Task.CompletedTask;
    }
}
