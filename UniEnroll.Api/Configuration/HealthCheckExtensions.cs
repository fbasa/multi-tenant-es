
using Microsoft.Extensions.DependencyInjection;

namespace UniEnroll.Api.Configuration;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthChecksExtensins(this IServiceCollection services)
    {
        services.AddHealthChecks();
        return services;
    }
}
