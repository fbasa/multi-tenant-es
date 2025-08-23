using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UniEnroll.Application.Abstractions;
using UniEnroll.Infrastructure.Common.Tenancy;

namespace UniEnroll.Api.Middleware;

public sealed class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITenantResolver _resolver;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<TenantResolutionMiddleware> _logger;
    private readonly UniEnroll.Api.Support.EfTenantSetter _efSetter;

    public TenantResolutionMiddleware(
        RequestDelegate next,
        ITenantResolver resolver,
        ITenantContext tenantContext,
        ILogger<TenantResolutionMiddleware> logger,
        UniEnroll.Api.Support.EfTenantSetter efSetter)
    {
        _next = next; _resolver = resolver; _tenantContext = tenantContext; _logger = logger; _efSetter = efSetter;
    }

    public async Task Invoke(HttpContext context)
    {
        var tid = await _resolver.ResolveAsync(context);
        if (!string.IsNullOrWhiteSpace(tid))
        {
            context.Items["TenantId"] = tid;
            (_tenantContext as UniEnroll.Infrastructure.Common.Tenancy.TenantContext)!.TenantId = tid;
            _efSetter.SetCurrentTenantId(tid); // set EF static holder
        }
        else
        {
            _logger.LogWarning("Tenant could not be resolved");
        }

        await _next(context);
    }
}
