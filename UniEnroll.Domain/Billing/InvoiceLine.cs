
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Billing;

public sealed class InvoiceLine
{
    public string Id { get; }
    public string InvoiceId { get; }
    public string Description { get; }
    public Money Amount { get; }

    public InvoiceLine(string id, string invoiceId, string description, Money amount)
    { Id = id; InvoiceId = invoiceId; Description = description; Amount = amount; }
}
