namespace UniEnroll.Contracts.Students;

public sealed record NotificationPrefsDto(bool EmailEnabled, bool SmsEnabled, bool PushEnabled);

