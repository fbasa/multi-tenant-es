
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace UniEnroll.Api.Auth;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.HasClaim("perm", requirement.Permission) ||
            context.User.HasClaim(ClaimTypes.Role, "Admin"))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
