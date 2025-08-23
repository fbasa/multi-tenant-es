
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Tenancy.Events;

public sealed class TenantUpserted : DomainEvent
{
    public string TenantId { get; }
    public TenantUpserted(string tenantId) { TenantId = tenantId; }
}
