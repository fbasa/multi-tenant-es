using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.Common.Tenancy;

public sealed class TenantContext : ITenantContext
{
    public string? TenantId { get; set; }
}
