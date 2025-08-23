
namespace UniEnroll.Domain.Tenancy;

public sealed class TenantFeatureFlag
{
    public string TenantId { get; }
    public string Key { get; }
    public bool Enabled { get; private set; }

    public TenantFeatureFlag(string tenantId, string key, bool enabled)
    {
        TenantId = tenantId; Key = key; Enabled = enabled;
    }

    public void Set(bool enabled) => Enabled = enabled;
}
