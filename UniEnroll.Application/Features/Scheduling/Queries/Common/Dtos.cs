
using System;

namespace UniEnroll.Application.Features.Scheduling.Queries.Common;

public sealed record ScheduleEntryDto(Guid SectionId, string CourseCode, string Room, int DayOfWeek, string StartTime, string EndTime);
public sealed record RoomConflictDto(Guid SectionId, string Room, int DayOfWeek, string StartTime, string EndTime, Guid ConflictsWithSectionId);
