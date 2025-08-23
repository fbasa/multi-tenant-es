
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Courses.Events;

public sealed class CourseCreated : DomainEvent
{
    public string CourseId { get; }
    public CourseCreated(string courseId) { CourseId = courseId; }
}
