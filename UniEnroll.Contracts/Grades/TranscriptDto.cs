namespace UniEnroll.Contracts.Grades;

public sealed record TranscriptDto(
    string StudentId,
    TranscriptLineDto[] Lines,
    decimal Gpa);
