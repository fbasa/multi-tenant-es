
using System;

namespace UniEnroll.Contracts.Grades;

public enum GradeOutcome
{
    Inserted,
    ValidationFailed
}

public sealed record GradeEntryDto(string Id, string EnrollmentId, decimal Points, DateTimeOffset RecordedAt);
