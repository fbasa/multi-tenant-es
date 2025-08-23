
using System;

namespace UniEnroll.Domain.Enrollment;

public sealed class EnrollmentAudit
{
    public string Id { get; }
    public string EnrollmentId { get; }
    public string Action { get; } // Enrolled/Waitlisted/Dropped/Promoted
    public string ActorUserId { get; }
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
    public string? Reason { get; }

    public EnrollmentAudit(string id, string enrollmentId, string action, string actorUserId, string? reason)
    {
        Id = id; EnrollmentId = enrollmentId; Action = action; ActorUserId = actorUserId; Reason = reason;
    }
}
