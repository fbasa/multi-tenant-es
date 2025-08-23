
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Domain.Abstractions;

public interface IDomainEventHandler<TEvent> where TEvent : DomainEvent
{
    Task HandleAsync(TEvent @event, CancellationToken ct = default);
}
