
using System;
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Enrollment;

public sealed class Enrollment : EntityBase, IAggregateRoot
{
    public string StudentId { get; private set; }
    public string SectionId { get; private set; }
    public EnrollmentStatus Status { get; private set; }
    public string TenantId { get; private set; }
    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    public Enrollment(string id, string studentId, string sectionId, string tenantId) : base(id)
    {
        StudentId = studentId;
        SectionId = sectionId;
        TenantId = tenantId;
        Status = EnrollmentStatus.Enrolled;
    }

    public void MarkWaitlisted() => Status = EnrollmentStatus.Waitlisted;
    public void Drop() => Status = EnrollmentStatus.Dropped;
}
