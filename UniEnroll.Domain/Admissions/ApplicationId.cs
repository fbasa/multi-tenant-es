
namespace UniEnroll.Domain.Admissions;

public readonly struct ApplicationId
{
    public string Value { get; }
    public ApplicationId(string value) => Value = value;
    public override string ToString() => Value;
}
