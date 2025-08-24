
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UniEnroll.Infrastructure.Common.Tenancy;

public sealed class CompositeTenantResolver : ITenantResolver
{
    private readonly HeaderTenantResolver _header;
    private readonly SubdomainTenantResolver _sub;
    private readonly TenantCache _cache;

    public CompositeTenantResolver(HeaderTenantResolver header, SubdomainTenantResolver sub, TenantCache cache)
    { 
        _header = header; 
        _sub = sub; 
        _cache = cache; 
    }

    public async Task<string?> ResolveAsync(HttpContext context)
    {
        var cached = _cache.TryGet(context, out var t) ? t : null;
        if (!string.IsNullOrWhiteSpace(cached)) return cached;

        var fromHeader = await _header.ResolveAsync(context);
        if (!string.IsNullOrWhiteSpace(fromHeader)) { _cache.Set(context, fromHeader!); return fromHeader; }

        var fromSub = await _sub.ResolveAsync(context);
        if (!string.IsNullOrWhiteSpace(fromSub)) { _cache.Set(context, fromSub!); return fromSub; }

        return null;
    }
}
