
using System;

namespace UniEnroll.Domain.Registrar;

public sealed class TranscriptRequest
{
    public string Id { get; }
    public string StudentId { get; }
    public string Status { get; private set; }
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;

    public TranscriptRequest(string id, string studentId, string status)
    { Id = id; StudentId = studentId; Status = status; }

    public void MarkProcessing() => Status = "Processing";
    public void MarkCompleted() => Status = "Completed";
}
