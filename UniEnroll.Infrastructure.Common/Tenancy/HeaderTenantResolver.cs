
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UniEnroll.Infrastructure.Common.Tenancy;

public sealed class HeaderTenantResolver : ITenantResolver
{
    private readonly IHttpContextAccessor _http;
    public HeaderTenantResolver(IHttpContextAccessor http) => _http = http;

    public Task<string?> ResolveAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(TenantHeaderNames.TenantId, out var v) && !string.IsNullOrWhiteSpace(v))
            return Task.FromResult<string?>(v.ToString());
        return Task.FromResult<string?>(null);
    }
}
