
using System;

namespace UniEnroll.Domain.Grades;

public sealed class GradeEntry
{
    public string Id { get; }
    public string EnrollmentId { get; }
    public decimal Points { get; private set; }
    public DateTimeOffset RecordedAt { get; } = DateTimeOffset.UtcNow;

    public GradeEntry(string id, string enrollmentId, decimal points)
    { Id = id; EnrollmentId = enrollmentId; Points = points; }
}
