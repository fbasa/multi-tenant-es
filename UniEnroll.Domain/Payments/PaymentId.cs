
namespace UniEnroll.Domain.Payments;

public readonly struct PaymentId
{
    public string Value { get; }
    public PaymentId(string value) => Value = value;
    public override string ToString() => Value;
}
