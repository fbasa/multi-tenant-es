
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Documents.Events;

public sealed class RequirementUploaded : DomainEvent
{
    public string RequirementId { get; }
    public RequirementUploaded(string requirementId) { RequirementId = requirementId; }
}
