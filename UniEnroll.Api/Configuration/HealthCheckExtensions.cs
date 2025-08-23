
using Microsoft.Extensions.DependencyInjection;

namespace UniEnroll.Api.Configuration;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthChecksBasic(this IServiceCollection services)
    {
        services.AddHealthChecks();
        return services;
    }
}
