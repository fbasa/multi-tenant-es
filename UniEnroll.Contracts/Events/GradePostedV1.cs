
using System;

namespace UniEnroll.Contracts.Events;

public sealed record GradePostedV1(string GradeId, string EnrollmentId, decimal Points, DateTimeOffset OccurredAt);
