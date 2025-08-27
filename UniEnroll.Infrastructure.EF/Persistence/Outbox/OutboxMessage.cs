
using System;

namespace UniEnroll.Infrastructure.EF.Persistence.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
    public string Type { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public string TenantId { get; set; } = default!;
    public string CorrelationId { get; set; } = default!;
    public bool Processed { get; set; }
    public DateTimeOffset? ProcessedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Error { get; set; }
}
