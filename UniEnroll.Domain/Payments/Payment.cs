
using System;
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Payments;

public sealed class Payment : EntityBase, IAggregateRoot
{
    public string InvoiceId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string TenantId { get; private set; }
    public DateTimeOffset CapturedAt { get; private set; }

    public Payment(string id, string invoiceId, Money amount, string tenantId) : base(id)
    {
        InvoiceId = invoiceId; Amount = amount; TenantId = tenantId; Status = PaymentStatus.Captured; CapturedAt = DateTimeOffset.UtcNow;
    }

    public void Refund() => Status = PaymentStatus.Refunded;
}
