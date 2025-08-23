
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Instructors.Events;

public sealed class InstructorUpserted : DomainEvent
{
    public string InstructorId { get; }
    public InstructorUpserted(string instructorId) { InstructorId = instructorId; }
}
