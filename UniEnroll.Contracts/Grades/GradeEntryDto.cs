
using System;

namespace UniEnroll.Contracts.Grades;

public sealed record GradeEntryDto(string Id, string EnrollmentId, decimal Points, DateTimeOffset RecordedAt);
