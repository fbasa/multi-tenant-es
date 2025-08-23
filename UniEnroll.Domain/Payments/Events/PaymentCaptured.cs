
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Payments.Events;

public sealed class PaymentCaptured : DomainEvent
{
    public string PaymentId { get; }
    public PaymentCaptured(string paymentId) { PaymentId = paymentId; }
}
