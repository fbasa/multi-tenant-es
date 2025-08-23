
namespace UniEnroll.Domain.Students;

public sealed class NotificationPreferences
{
    public bool EmailEnabled { get; private set; }
    public bool SmsEnabled { get; private set; }
    public bool PushEnabled { get; private set; }

    public NotificationPreferences(bool email, bool sms, bool push)
    { EmailEnabled = email; SmsEnabled = sms; PushEnabled = push; }

    public void Update(bool email, bool sms, bool push)
    { EmailEnabled = email; SmsEnabled = sms; PushEnabled = push; }
}
