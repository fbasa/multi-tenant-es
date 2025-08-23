
using System;

namespace UniEnroll.Contracts.Events;

public sealed record RequirementUploadedV1(string RequirementId, string StudentId, string Type, DateTimeOffset OccurredAt);
