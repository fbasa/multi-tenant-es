
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Registrar;

public sealed class GraduationAudit : EntityBase, IAggregateRoot
{
    public string StudentId { get; private set; }
    public bool Eligible { get; private set; }
    public string[] MissingCourseIds { get; private set; }
    public string TenantId { get; private set; }

    public GraduationAudit(string id, string studentId, bool eligible, string[] missingCourseIds, string tenantId) : base(id)
    {
        StudentId = studentId; Eligible = eligible; MissingCourseIds = missingCourseIds ?? System.Array.Empty<string>(); TenantId = tenantId;
    }
}
