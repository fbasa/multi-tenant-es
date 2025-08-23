using System;

namespace UniEnroll.Contracts.Sections;

public sealed record SectionDto(
    string Id,
    string CourseId,
    string TermId,
    string InstructorId,
    int Capacity,
    int WaitlistCapacity,
    string? Room,
    DayOfWeek[] Days,
    TimeSpan Start,
    TimeSpan End);
