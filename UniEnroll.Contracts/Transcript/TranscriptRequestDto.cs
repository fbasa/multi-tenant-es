
using System;

namespace UniEnroll.Contracts.Transcript;

public sealed record TranscriptRequestDto(string Id, string StudentId, string Status, DateTimeOffset CreatedAt);
