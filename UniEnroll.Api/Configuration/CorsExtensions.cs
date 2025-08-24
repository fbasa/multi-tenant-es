
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UniEnroll.Api.Configuration;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsExtensions(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors(opt =>
        {
            var origins = (config["Cors:Origins"] ?? "https://localhost").Split(';', System.StringSplitOptions.RemoveEmptyEntries);
            opt.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyMethod().WithOrigins(origins).AllowCredentials());
        });
        return services;
    }
}
