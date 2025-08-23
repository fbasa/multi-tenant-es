
namespace UniEnroll.Domain.Billing.ValueObjects;

public readonly struct Amount
{
    public decimal Value { get; }
    public string Currency { get; }
    public Amount(decimal value, string currency = "PHP") { Value = value; Currency = string.IsNullOrWhiteSpace(currency) ? "PHP" : currency; }
    public override string ToString() => $"{Currency} {Value:0.00}";
}
