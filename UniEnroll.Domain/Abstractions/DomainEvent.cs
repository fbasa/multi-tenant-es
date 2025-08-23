
using System;

namespace UniEnroll.Domain.Abstractions;

public abstract class DomainEvent
{
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}
