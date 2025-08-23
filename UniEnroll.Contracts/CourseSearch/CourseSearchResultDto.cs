namespace UniEnroll.Contracts.CourseSearch;

public sealed record CourseSearchResultDto(
    string CourseId,
    string SectionId,
    string Code,
    string Title,
    int Units,
    string InstructorName,
    int Capacity,
    int SeatsTaken,
    int WaitlistCount);
