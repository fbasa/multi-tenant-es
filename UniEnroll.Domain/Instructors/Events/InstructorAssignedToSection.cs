
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Instructors.Events;

public sealed class InstructorAssignedToSection : DomainEvent
{
    public string InstructorId { get; }
    public string SectionId { get; }
    public InstructorAssignedToSection(string instructorId, string sectionId) { InstructorId = instructorId; SectionId = sectionId; }
}
