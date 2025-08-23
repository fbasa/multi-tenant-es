namespace UniEnroll.Infrastructure.Common.Idempotency;

public sealed class IdempotencyOptions
{
    public int KeyMaxLength { get; set; } = 128;
    public int TtlMinutes { get; set; } = 10;
}
