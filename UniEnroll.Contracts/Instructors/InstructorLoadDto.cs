namespace UniEnroll.Contracts.Instructors;

public sealed record InstructorLoadDto(
    string InstructorId,
    int Units,
    int Sections);
