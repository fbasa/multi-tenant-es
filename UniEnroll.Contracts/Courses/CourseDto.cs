namespace UniEnroll.Contracts.Courses;

public sealed record CourseDto(
    string Id,
    string Code,
    string Title,
    int Units,
    string Department);
