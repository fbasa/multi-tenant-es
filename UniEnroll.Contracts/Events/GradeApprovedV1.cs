
using System;

namespace UniEnroll.Contracts.Events;

public sealed record GradeApprovedV1(string GradeId, string ApprovedBy, DateTimeOffset OccurredAt);
