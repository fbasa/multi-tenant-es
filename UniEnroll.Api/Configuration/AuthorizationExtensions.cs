
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using UniEnroll.Api.Auth;

namespace UniEnroll.Api.Configuration;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(o =>
        {
            o.AddPolicy(Policies.Student, p => p.Requirements.Add(new PermissionRequirement(Permissions.Student.Read)));
            o.AddPolicy(Policies.Instructor, p => p.Requirements.Add(new PermissionRequirement(Permissions.Instructor.Read)));
            o.AddPolicy(Policies.Registrar, p => p.Requirements.Add(new PermissionRequirement(Permissions.Registrar.Read)));
            o.AddPolicy(Policies.Reporting, p => p.Requirements.Add(new PermissionRequirement(Permissions.Reporting.Read)));
        });
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        return services;
    }
}
