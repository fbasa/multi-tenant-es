
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Enrollment.Events;

public sealed class PromotedFromWaitlist : DomainEvent
{
    public string EnrollmentId { get; }
    public PromotedFromWaitlist(string enrollmentId) { EnrollmentId = enrollmentId; }
}
