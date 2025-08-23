
using System;

namespace UniEnroll.Contracts.Events;

public sealed record PromotedFromWaitlistV1(string EnrollmentId, string SectionId, string StudentId, DateTimeOffset OccurredAt);
