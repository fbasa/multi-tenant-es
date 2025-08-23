namespace UniEnroll.Contracts.Grades;

public sealed record TranscriptLineDto(
    string TermId,
    string CourseCode,
    string CourseTitle,
    int Units,
    string Grade);
