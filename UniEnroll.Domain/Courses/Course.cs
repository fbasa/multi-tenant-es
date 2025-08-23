
using UniEnroll.Domain.Common;
using UniEnroll.Domain.Courses.ValueObjects;

namespace UniEnroll.Domain.Courses;

public sealed class Course : EntityBase, IAggregateRoot
{
    public CourseCode Code { get; private set; }
    public string Title { get; private set; }
    public CreditUnit Units { get; private set; }
    public string TenantId { get; private set; }

    public Course(string id, CourseCode code, string title, CreditUnit units, string tenantId) : base(id)
    {
        Code = code;
        Title = title;
        Units = units;
        TenantId = tenantId;
    }
}
