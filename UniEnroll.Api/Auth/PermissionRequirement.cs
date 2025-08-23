
using Microsoft.AspNetCore.Authorization;

namespace UniEnroll.Api.Auth;

public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }
    public PermissionRequirement(string permission) => Permission = permission;
}
