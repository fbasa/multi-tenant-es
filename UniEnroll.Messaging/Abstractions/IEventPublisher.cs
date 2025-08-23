namespace UniEnroll.Messaging.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, CancellationToken ct = default);
}