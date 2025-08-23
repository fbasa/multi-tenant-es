
using UniEnroll.Domain.Abstractions;

namespace UniEnroll.Domain.Identity.Events;

public sealed class UserRegistered : DomainEvent
{
    public string UserId { get; }
    public string TenantId { get; }
    public UserRegistered(string userId, string tenantId) { UserId = userId; TenantId = tenantId; }
}
