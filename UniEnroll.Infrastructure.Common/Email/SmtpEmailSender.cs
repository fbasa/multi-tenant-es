
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using UniEnroll.Application.Abstractions;
using UniEnroll.Infrastructure.Common.Options;

namespace UniEnroll.Infrastructure.Common.Email;

public sealed class SmtpEmailSender : IEmailSender
{
    private readonly SmtpOptions _options;
    public SmtpEmailSender(IOptions<SmtpOptions> options) => _options = options.Value;

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        using var client = new SmtpClient(_options.Host, _options.Port) { EnableSsl = _options.EnableSsl };
        if (!string.IsNullOrWhiteSpace(_options.User))
            client.Credentials = new NetworkCredential(_options.User, _options.Password);
        using var msg = new MailMessage(_options.From, to, subject, htmlBody) { IsBodyHtml = true };
        await client.SendMailAsync(msg);
    }
}
