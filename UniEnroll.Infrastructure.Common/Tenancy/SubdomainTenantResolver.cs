
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UniEnroll.Infrastructure.Common.Tenancy;

public sealed class SubdomainTenantResolver : ITenantResolver
{
    public Task<string?> ResolveAsync(HttpContext context)
    {
        var host = context.Request.Host.Host;
        var parts = host.Split('.');
        return Task.FromResult<string?>(parts.Length >= 3 ? parts[0] : null);
    }
}
