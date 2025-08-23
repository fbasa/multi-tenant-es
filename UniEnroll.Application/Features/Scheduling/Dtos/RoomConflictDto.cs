
using System;

namespace UniEnroll.Application.Features.Scheduling.Dtos;

public sealed record RoomConflictDto(string SectionAId, string SectionBId, string RoomCode, string TermId);
