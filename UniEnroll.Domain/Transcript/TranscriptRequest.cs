
using System;
using UniEnroll.Domain.Common;

namespace UniEnroll.Domain.Transcript;

public sealed class TranscriptRequest : EntityBase, IAggregateRoot
{
    public string StudentId { get; private set; }
    public string Status { get; private set; } // Pending/Processing/Completed
    public string TenantId { get; private set; }

    public TranscriptRequest(string id, string studentId, string status, string tenantId) : base(id)
    {
        StudentId = studentId; Status = status; TenantId = tenantId;
    }

    public void MarkProcessing() => Status = "Processing";
    public void MarkCompleted() => Status = "Completed";
}
