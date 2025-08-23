
namespace UniEnroll.Domain.Billing;

public readonly struct InvoiceId
{
    public string Value { get; }
    public InvoiceId(string value) => Value = value;
    public override string ToString() => Value;
}
