
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Sections.Events;

public sealed class SectionCreated : DomainEvent
{
    public string SectionId { get; }
    public SectionCreated(string sectionId) { SectionId = sectionId; }
}
