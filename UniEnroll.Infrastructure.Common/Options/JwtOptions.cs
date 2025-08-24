namespace UniEnroll.Infrastructure.Common.Options;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "UniEnroll";
    public string Audience { get; set; } = "UniEnroll.Clients";
    public string SigningKey { get; set; } = "dev-signing-key-change-me";
    public int AccessTokenMinutes { get; set; }
}
