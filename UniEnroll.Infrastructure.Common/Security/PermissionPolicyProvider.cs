
using Microsoft.AspNetCore.Authorization;

namespace UniEnroll.Infrastructure.Common.Security;

/// <summary>Helper to build policies; API should register them.</summary>
public static class PermissionPolicyProvider
{
    public static AuthorizationPolicy Build(string permission)
        => new AuthorizationPolicyBuilder().RequireClaim("perm", permission).Build();
}
