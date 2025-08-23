
namespace UniEnroll.Infrastructure.Common.Options;

public sealed class SmtpOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 25;
    public bool EnableSsl { get; set; } = false;
    public string? User { get; set; }
    public string? Password { get; set; }
    public string From { get; set; } = "no-reply@unienroll.local";
}
