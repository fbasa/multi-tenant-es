
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Documents;

public sealed class Requirement : EntityBase, IAggregateRoot
{
    public string StudentId { get; private set; }
    public string Type { get; private set; } // e.g., PSA, TOR, etc.
    public string Status { get; private set; } // Submitted/Approved/Rejected
    public string? ReviewerNotes { get; private set; }
    public string TenantId { get; private set; }

    public Requirement(string id, string studentId, string type, string status, string tenantId) : base(id)
    {
        StudentId = studentId; Type = type; Status = status; TenantId = tenantId;
    }

    public void Review(string status, string? notes) { Status = status; ReviewerNotes = notes; }
}
