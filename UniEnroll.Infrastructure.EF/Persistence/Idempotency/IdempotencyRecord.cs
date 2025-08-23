
using System;

namespace UniEnroll.Infrastructure.EF.Persistence.Idempotency;

public sealed class IdempotencyRecord
{
    public string Key { get; set; } = default!;
    public string Hash { get; set; } = default!;
    public DateTimeOffset ExpiresAt { get; set; }
}
