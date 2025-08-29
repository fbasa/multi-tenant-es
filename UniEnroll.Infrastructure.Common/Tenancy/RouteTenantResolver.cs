using Microsoft.AspNetCore.Http;
using UniEnroll.Infrastructure.Common.Abstractions;

namespace UniEnroll.Infrastructure.Common.Tenancy;

public sealed class RouteTenantResolver : ITenantResolver
{
    public Task<string?> ResolveAsync(HttpContext httpContext)
    {
        //// Endpoint routing must have run to populate RouteValues
        //if (httpContext.Request.RouteValues.TryGetValue(TenantHeaderNames.TenantId, out var value) &&
        //    value is string s && !string.IsNullOrWhiteSpace(s))
        //{
        //    return Task.FromResult<string?>(s);
        //}
        //return Task.FromResult<string?>(null);
        throw new NotImplementedException();
    }
}
