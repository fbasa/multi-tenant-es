
using System;

namespace UniEnroll.Contracts.Events;

public sealed record EnrollmentConfirmedV1(string EnrollmentId, DateTimeOffset OccurredAt);
