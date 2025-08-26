namespace UniEnroll.Contracts.Payments;

public enum PaymentOutcome
{
    Captured,
    AlreadyCaptured,
    Declined,
    Refunded,
    NotFound,
    Conflict,
    ValidationFailed
}

public sealed record CapturePaymentResult(PaymentOutcome Outcome, string? PaymentId);
public sealed record RefundPaymentResult(PaymentOutcome Outcome, string? RefundPaymentId);
public sealed record PaymentStatusResult(string PaymentId, string Status, decimal Amount, string Currency);

