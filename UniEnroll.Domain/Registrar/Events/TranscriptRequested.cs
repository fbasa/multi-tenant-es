
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Registrar.Events;

public sealed class TranscriptRequested : DomainEvent
{
    public string RequestId { get; }
    public TranscriptRequested(string requestId) { RequestId = requestId; }
}
