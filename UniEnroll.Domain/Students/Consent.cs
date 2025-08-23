
using System;

namespace UniEnroll.Domain.Students;

public sealed class Consent
{
    public string StudentId { get; }
    public string Type { get; }
    public bool Granted { get; private set; }
    public DateTimeOffset GrantedAt { get; private set; }

    public Consent(string studentId, string type, bool granted)
    {
        StudentId = studentId; Type = type; Granted = granted; GrantedAt = DateTimeOffset.UtcNow;
    }

    public void Set(bool granted) { Granted = granted; GrantedAt = DateTimeOffset.UtcNow; }
}
