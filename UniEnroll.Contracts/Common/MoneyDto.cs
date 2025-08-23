namespace UniEnroll.Contracts.Common;

/// <summary>Represents a monetary amount. Default currency is PHP.</summary>
public sealed record MoneyDto(decimal Amount, string Currency = "PHP");
