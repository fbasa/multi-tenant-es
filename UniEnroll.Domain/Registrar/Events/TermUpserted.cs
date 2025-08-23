
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Registrar.Events;

public sealed class TermUpserted : DomainEvent
{
    public string TermId { get; }
    public TermUpserted(string termId) { TermId = termId; }
}
