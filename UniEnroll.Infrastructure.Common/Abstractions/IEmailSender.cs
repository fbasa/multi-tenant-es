
using System.Threading;
using System.Threading.Tasks;

namespace UniEnroll.Infrastructure.Common.Abstractions;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
}
