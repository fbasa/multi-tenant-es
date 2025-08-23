namespace UniEnroll.Messaging.SendGrid;

public interface IEmailSender
{
    Task SendAsync(EmailMessage msg, CancellationToken ct);
}
