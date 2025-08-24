namespace UniEnroll.Contracts.Payments;

public sealed record RefundPaymentRequest(Guid PaymentId,
    decimal Amount,
    string Reason,
    string? IdempotencyKey
    );
