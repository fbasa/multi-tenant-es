
namespace UniEnroll.Domain.Common;

/// <summary>Simple money value object (currency default: PHP)</summary>
public readonly struct Money
{
    public decimal Amount { get; }
    public string Currency { get; }
    public Money(decimal amount, string currency = "PHP") { Amount = amount; Currency = string.IsNullOrWhiteSpace(currency) ? "PHP" : currency; }
    public override string ToString() => $"{Currency} {Amount:0.00}";
}
