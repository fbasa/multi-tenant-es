
using Microsoft.AspNetCore.RateLimiting;

namespace UniEnroll.Api.Configuration;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddRateLimitingExtensions(this IServiceCollection services, IConfiguration config)
    {
        services.AddRateLimiter(_ => _.AddFixedWindowLimiter("fixed", opt =>
        {
            opt.Window = TimeSpan.FromSeconds(1);
            opt.PermitLimit = 50;
            opt.QueueLimit = 0;
        }));
        return services;
    }
}
