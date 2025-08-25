
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using UniEnroll.Infrastructure.Common.Auth;

namespace UniEnroll.Infrastructure.Common.Security;

/// <summary>Helper to build policies; API should register them.</summary>
public sealed class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly ConcurrentDictionary<string, AuthorizationPolicy> _cache = new();
    private readonly AuthorizationOptions _options;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) => _options = options.Value;

    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // Return if already registered statically
        if (_options.GetPolicy(policyName) is { } existing)
        {
            return Task.FromResult<AuthorizationPolicy?>(existing);
        }

        // Only allow know permissions (from PermissionRegistry)
        if (!PermissionRegistry.IsKnown(policyName))
        {
            return Task.FromResult<AuthorizationPolicy?>(null);
        }

        var policy = _cache.GetOrAdd(policyName, name =>
                        new AuthorizationPolicyBuilder()
                        .AddRequirements(new PermissionRequirement(name))
                        .Build());

        return Task.FromResult<AuthorizationPolicy?>(policy);

    }
}
