
namespace UniEnroll.Domain.Students.ValueObjects;

public readonly struct NotificationPrefs
{
    public bool EmailEnabled { get; }
    public bool SmsEnabled { get; }
    public bool PushEnabled { get; }
    public NotificationPrefs(bool emailEnabled, bool smsEnabled, bool pushEnabled)
    {
        EmailEnabled = emailEnabled; SmsEnabled = smsEnabled; PushEnabled = pushEnabled;
    }
}
