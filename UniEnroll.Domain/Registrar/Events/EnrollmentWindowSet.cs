
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Registrar.Events;

public sealed class EnrollmentWindowSet : DomainEvent
{
    public string TermId { get; }
    public System.DateTimeOffset OpensAt { get; }
    public System.DateTimeOffset ClosesAt { get; }
    public EnrollmentWindowSet(string termId, System.DateTimeOffset opensAt, System.DateTimeOffset closesAt)
    { TermId = termId; OpensAt = opensAt; ClosesAt = closesAt; }
}
