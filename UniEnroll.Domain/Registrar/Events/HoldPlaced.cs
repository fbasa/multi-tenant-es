
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Registrar.Events;

public sealed class HoldPlaced : DomainEvent
{
    public string HoldId { get; }
    public HoldPlaced(string holdId) { HoldId = holdId; }
}
