
using System;

namespace UniEnroll.Infrastructure.EF.Persistence.Outbox;

public sealed class OutboxMessage
{
    public string Id { get; set; } = default!;
    public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
    public string Type { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public bool Processed { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }
}
