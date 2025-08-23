
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Billing;

public sealed class Invoice : EntityBase, IAggregateRoot
{
    public string StudentId { get; private set; }
    public Money Amount { get; private set; }
    public string TermId { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public string TenantId { get; private set; }

    public Invoice(string id, string studentId, Money amount, string termId, string tenantId) : base(id)
    {
        StudentId = studentId; Amount = amount; TermId = termId; TenantId = tenantId; Status = InvoiceStatus.Draft;
    }

    public void MarkPosted() => Status = InvoiceStatus.Posted;
    public void MarkPaid() => Status = InvoiceStatus.Paid;
    public void MarkVoided() => Status = InvoiceStatus.Voided;
}
