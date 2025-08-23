
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Application.Abstractions;

/// <summary>Simple pub/sub abstraction for domain/integration events.</summary>
public interface IEventBus
{
    Task PublishAsync<T>(T @event, CancellationToken ct = default);
}
