
using Microsoft.AspNetCore.Http;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.Common.Tenancy;

public sealed class CompositeTenantResolver(
        ITenantResolver[] resolver,
        ITenantCache cache
    ) : ITenantResolver
{
    public async Task<string?> ResolveAsync(HttpContext context)
    {
        var cached = cache.TryGet(context, out var t) ? t : null;
        if (!string.IsNullOrWhiteSpace(cached))
        {
            return cached;
        }
        foreach (var resolver in resolver)
        {
            var tenantId = await resolver.ResolveAsync(context);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                cache.Set(context, tenantId!);
                return tenantId;
            }
        }
        return null;
    }
}
