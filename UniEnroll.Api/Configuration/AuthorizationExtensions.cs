
using Microsoft.AspNetCore.Authorization;
using UniEnroll.Api.Auth;
using UniEnroll.Infrastructure.Common.Auth;
using UniEnroll.Infrastructure.Common.Security;

namespace UniEnroll.Api.Configuration;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationExtensions(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        services.AddAuthorization(o =>
        {
            foreach (var perm in PermissionRegistry.All)
                o.AddPolicy(perm, p => p.Requirements.Add(new PermissionRequirement(perm)));
        });

        services.AddAuthorization();

        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

        return services;
    }
}
