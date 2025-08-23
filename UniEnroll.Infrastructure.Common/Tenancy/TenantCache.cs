
using Microsoft.AspNetCore.Http;

namespace UniEnroll.Infrastructure.Common.Tenancy;

public sealed class TenantCache
{
    private const string Key = "__tenantId";
    public bool TryGet(HttpContext ctx, out string? tenantId)
    {
        tenantId = ctx.Items.TryGetValue(Key, out var v) ? v?.ToString() : null;
        return !string.IsNullOrWhiteSpace(tenantId);
    }
    public void Set(HttpContext ctx, string tenantId) => ctx.Items[Key] = tenantId;
}
