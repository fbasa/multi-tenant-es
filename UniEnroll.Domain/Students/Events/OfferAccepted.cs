
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Students.Events;

public sealed class OfferAccepted : DomainEvent
{
    public string StudentId { get; }
    public string ProgramId { get; }
    public OfferAccepted(string studentId, string programId) { StudentId = studentId; ProgramId = programId; }
}
