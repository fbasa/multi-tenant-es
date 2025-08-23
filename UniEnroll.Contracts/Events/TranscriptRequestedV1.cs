
using System;

namespace UniEnroll.Contracts.Events;

public sealed record TranscriptRequestedV1(string RequestId, string StudentId, DateTimeOffset OccurredAt);
