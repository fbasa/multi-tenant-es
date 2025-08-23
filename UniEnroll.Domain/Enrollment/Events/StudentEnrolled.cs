
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Enrollment.Events;

public sealed class StudentEnrolled : DomainEvent
{
    public string EnrollmentId { get; }
    public StudentEnrolled(string enrollmentId) { EnrollmentId = enrollmentId; }
}
