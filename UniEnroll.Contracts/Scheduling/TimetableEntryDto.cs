using System;

namespace UniEnroll.Contracts.Scheduling;

public sealed record TimetableEntryDto(
    string SectionId,
    string CourseCode,
    string Title,
    DayOfWeek[] Days,
    TimeSpan Start,
    TimeSpan End,
    string Room);
