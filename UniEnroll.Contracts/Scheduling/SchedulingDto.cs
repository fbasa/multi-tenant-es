namespace UniEnroll.Contracts.Scheduling;

//public sealed record RoomConflictDto(string SectionAId, string SectionBId, string RoomCode, string TermId);

public sealed record TimetableDto(
    string OwnerId,
    string TermId,
    TimetableEntryDto[] Entries);

public sealed record TimetableEntryDto(
    string SectionId,
    string CourseCode,
    string Title,
    DayOfWeek[] Days,
    TimeSpan Start,
    TimeSpan End,
    string Room);
public sealed record ScheduleEntryDto(Guid SectionId, string CourseCode, string Room, int DayOfWeek, string StartTime, string EndTime);
public sealed record RoomConflictDto(Guid SectionId, string Room, int DayOfWeek, string StartTime, string EndTime, Guid ConflictsWithSectionId);
