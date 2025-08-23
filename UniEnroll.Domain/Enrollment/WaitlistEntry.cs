
using System;
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Enrollment;

/// <summary>Represents a student's position in a section waitlist (FIFO).</summary>
public sealed class WaitlistEntry : EntityBase
{
    public string SectionId { get; }
    public string StudentId { get; }
    public string TenantId { get; }
    public int Position { get; private set; }

    public WaitlistEntry(string id, string sectionId, string studentId, int position, string tenantId) : base(id)
    {
        SectionId = sectionId; StudentId = studentId; Position = position; TenantId = tenantId;
    }

    public void Promote() => Position = 0; // 0 indicates promoted/cleared in this simple model
}
