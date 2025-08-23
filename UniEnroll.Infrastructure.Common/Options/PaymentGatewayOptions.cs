
namespace UniEnroll.Infrastructure.Common.Options;

public sealed class PaymentGatewayOptions
{
    public string Provider { get; set; } = "Mock";
    public string ApiKey { get; set; } = string.Empty;
    public string? Secret { get; set; }
}
