using System.Threading;
using System.Threading.Tasks;
using UniEnroll.Application.Abstractions;

namespace UniEnroll.Infrastructure.Common.Email;

public sealed class NoOpEmailSender : IEmailSender
{
    public Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default) => Task.CompletedTask;
}
