using System;

namespace UniEnroll.Contracts.Enrollment;

public sealed record EnrollmentDto(
    string EnrollmentId,
    string StudentId,
    string SectionId,
    string Status,
    DateTimeOffset CreatedAt);
