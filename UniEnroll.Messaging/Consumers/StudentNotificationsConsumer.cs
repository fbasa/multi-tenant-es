
using System.Text.Json;
using Microsoft.Extensions.Logging;
using UniEnroll.Messaging.Abstractions;
using UniEnroll.Contracts.Events;

namespace UniEnroll.Messaging.Consumers;

/// <summary>Sends lightweight student notifications for common events.</summary>
public sealed class StudentNotificationsConsumer : IEventConsumer
{
    public IReadOnlyCollection<string> Topics { get; } = new[]
    {
        "enrollment.confirmed",
        "grades.posted",
        "transcript.requested",
        "requirements.uploaded"
    };

    private readonly ILogger<StudentNotificationsConsumer> _logger;

    public StudentNotificationsConsumer(ILogger<StudentNotificationsConsumer> logger) => _logger = logger;

    public Task HandleAsync(ReadOnlyMemory<byte> body, string routingKey, IDictionary<string, object?> headers, CancellationToken ct)
    {
        _logger.LogInformation("Student notification for event {Key} (size={Size})", routingKey, body.Length);
        return Task.CompletedTask;
    }
}
