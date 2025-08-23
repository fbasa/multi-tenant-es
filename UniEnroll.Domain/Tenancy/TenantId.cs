
namespace UniEnroll.Domain.Tenancy;

public readonly struct TenantId
{
    public string Value { get; }
    public TenantId(string value) => Value = value;
    public override string ToString() => Value;
}
