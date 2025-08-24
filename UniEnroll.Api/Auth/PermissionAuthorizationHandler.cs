
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UniEnroll.Application.Abstractions;

namespace UniEnroll.Api.Auth;

public sealed class PermissionAuthorizationHandler(ITenantContext tenant, IPermissionService perms) : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userId = context.User.FindFirst("sub")?.Value;

        if (context.User.HasClaim("perm", requirement.Permission) ||
            context.User.HasClaim(ClaimTypes.Role, "Admin"))
        {
            context.Succeed(requirement);
        }

        // 3) DB lookup with caching (role permission map per tenant)
        if (userId is not null && tenant.TenantId is not null &&
            await perms.HasPermissionAsync(tenant.TenantId, userId, requirement.Permission, default))
        { 
            context.Succeed(requirement); 
        }
    }
}
