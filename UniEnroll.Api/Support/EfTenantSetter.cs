using System.Reflection;

namespace UniEnroll.Api.Support;

/// <summary>
/// Sets EF's internal static TenantId holder via reflection to avoid tight coupling.
/// </summary>
public sealed class EfTenantSetter
{
    private readonly PropertyInfo? _holder;

    public EfTenantSetter()
    {
        var t = Type.GetType("UniEnroll.Infrastructure.EF.Persistence.QueryFilters+TenantIdHolder, UniEnroll.Infrastructure.EF");
        _holder = t?.GetProperty("CurrentTenantId", BindingFlags.Public | BindingFlags.Static);
    }

    public void SetCurrentTenantId(string? tenantId)
        => _holder?.SetValue(null, tenantId);
}
