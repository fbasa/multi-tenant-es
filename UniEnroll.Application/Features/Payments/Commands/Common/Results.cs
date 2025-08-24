
namespace UniEnroll.Application.Features.Payments.Commands.Common;

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
