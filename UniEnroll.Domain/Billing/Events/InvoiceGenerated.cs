
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Billing.Events;

public sealed class InvoiceGenerated : DomainEvent
{
    public string InvoiceId { get; }
    public InvoiceGenerated(string invoiceId) { InvoiceId = invoiceId; }
}
