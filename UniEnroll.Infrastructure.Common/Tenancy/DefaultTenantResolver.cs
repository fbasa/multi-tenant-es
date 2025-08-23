using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UniEnroll.Infrastructure.Common.Tenancy;

/// <summary>Resolves tenant id from header 'X-Tenant-Id' or query 'tenantId'.</summary>
public sealed class DefaultTenantResolver : ITenantResolver
{
    public Task<string?> ResolveAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue("X-Tenant-Id", out var fromHeader) && !string.IsNullOrWhiteSpace(fromHeader))
            return Task.FromResult<string?>(fromHeader.ToString());

        if (httpContext.Request.Query.TryGetValue("tenantId", out var fromQuery) && !string.IsNullOrWhiteSpace(fromQuery))
            return Task.FromResult<string?>(fromQuery.ToString());

        return Task.FromResult<string?>(null);
    }
}
