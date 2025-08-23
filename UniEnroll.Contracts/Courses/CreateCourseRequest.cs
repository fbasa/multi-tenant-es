
namespace UniEnroll.Contracts.Courses;

public sealed record CreateCourseRequest(string Code, string Title, int Units, string Department);
