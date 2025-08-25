
using Microsoft.AspNetCore.Authorization;

namespace UniEnroll.Infrastructure.Common.Auth;

public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }
    public PermissionRequirement(string permission) => Permission = permission;
}
