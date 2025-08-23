using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniEnroll.Application.Abstractions;
using UniEnroll.Infrastructure.Common.Auth;
using UniEnroll.Infrastructure.Common.Email;
using UniEnroll.Infrastructure.Common.Files;
using UniEnroll.Infrastructure.Common.Idempotency;
using UniEnroll.Infrastructure.Common.Ids;
using UniEnroll.Infrastructure.Common.Options;
using UniEnroll.Infrastructure.Common.Tenancy;
using UniEnroll.Infrastructure.Common.Time;

namespace UniEnroll.Infrastructure.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureCommon(this IServiceCollection services, IConfiguration config)
    {
        //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();    //TODO

        // Options
        services.Configure<ApiLimitsOptions>(config.GetSection("ApiLimits"));
        services.Configure<IdempotencyOptions>(config.GetSection("Idempotency"));
        services.Configure<StorageOptions>(config.GetSection("Storage"));
        services.Configure<JwtOptions>(config.GetSection("Jwt"));

        // Core services
        services.AddScoped<ICurrentUser, HttpCurrentUser>();
        services.AddScoped<ITenantContext, TenantContext>();
        services.AddSingleton<IDateTimeProvider, SystemClock>();
        services.AddSingleton<IIdGenerator, IdGenerator>();

        // Idempotency
        services.AddSingleton<IIdempotencyStore, InMemoryIdempotencyStore>();

        // Email + Files
        services.AddSingleton<IEmailSender, NoOpEmailSender>();
        services.AddSingleton<IFileStorage, LocalFileStorage>();

        // Tenant resolver
        services.AddSingleton<ITenantResolver, DefaultTenantResolver>();

        return services;
    }
}
