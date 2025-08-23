
using UniEnroll.Domain.Common;
using UniEnroll.Domain.Students.ValueObjects;

namespace UniEnroll.Domain.Students;

public sealed class Student : EntityBase, IAggregateRoot
{
    public StudentName Name { get; private set; }
    public string Email { get; private set; }
    public string ProgramId { get; private set; }
    public int EntryYear { get; private set; }
    public string TenantId { get; private set; }
    public NotificationPrefs Notification { get; private set; }

    public Student(string id, StudentName name, string email, string programId, int entryYear, string tenantId) : base(id)
    {
        Name = name;
        Email = email;
        ProgramId = programId;
        EntryYear = entryYear;
        TenantId = tenantId;
        Notification = new NotificationPrefs(true, false, false);
    }

    public void UpdateNotificationPrefs(bool email, bool sms, bool push) =>
        Notification = new NotificationPrefs(email, sms, push);
}
