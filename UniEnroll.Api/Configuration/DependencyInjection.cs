
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace UniEnroll.Api.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApiCore(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddApiVersioningV1()
            .AddProblemDetailsWithCorrelation()
            .AddCorsPolicy(config)
            .AddSwaggerDocs()
            .AddAuth(config)
            .AddAuthorizationPolicies()
            .AddHealthChecksBasic()
            .AddRateLimiting(config)
            .AddDataProtectionKeys(config);

        return services;
    }
    //TODO
    //public static IApplicationBuilder UseApiCore(this IApplicationBuilder app)
    //{
    //    app.UseRateLimiter();
    //    app.UseCorrelationId();
    //    app.UseTenantResolution();
    //    app.UseIdempotency();
    //    app.UseExceptionHandling();
    //    app.UseRequestLogging();
    //    return app;
    //}
}
