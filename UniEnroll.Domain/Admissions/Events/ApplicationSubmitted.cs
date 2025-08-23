
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Admissions.Events;

public sealed class ApplicationSubmitted : DomainEvent
{
    public string ApplicationId { get; }
    public ApplicationSubmitted(string applicationId) { ApplicationId = applicationId; }
}
