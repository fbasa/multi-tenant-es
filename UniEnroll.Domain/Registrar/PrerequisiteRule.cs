
namespace UniEnroll.Domain.Registrar;

public sealed class PrerequisiteRule
{
    public string CourseId { get; }
    public string[] PrereqCourseIds { get; }
    public PrerequisiteRule(string courseId, string[] prereqCourseIds)
    { CourseId = courseId; PrereqCourseIds = prereqCourseIds ?? System.Array.Empty<string>(); }
}
