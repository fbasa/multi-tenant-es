
using System;

namespace UniEnroll.Contracts.Events;

public sealed record InstructorAssignedV1(string InstructorId, string SectionId, DateTimeOffset OccurredAt);
