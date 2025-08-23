
namespace UniEnroll.Domain.Documents;

public readonly struct RequirementId
{
    public string Value { get; }
    public RequirementId(string value) => Value = value;
    public override string ToString() => Value;
}
