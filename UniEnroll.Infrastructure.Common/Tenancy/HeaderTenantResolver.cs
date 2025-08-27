
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.Common.Tenancy;

public sealed class HeaderTenantResolver : ITenantResolver
{
    public Task<string?> ResolveAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(TenantHeaderNames.TenantId, out var fromHeader) && !string.IsNullOrWhiteSpace(fromHeader))
            return Task.FromResult<string?>(fromHeader.ToString());
        return Task.FromResult<string?>(null);
    }
}
