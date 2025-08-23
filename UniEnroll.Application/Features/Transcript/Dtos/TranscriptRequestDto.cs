
using System;

namespace UniEnroll.Application.Features.Transcript.Dtos;

public sealed record TranscriptRequestDto(string Id, string StudentId, string Status, DateTimeOffset CreatedAt);
