
namespace UniEnroll.Domain.Sections;

public readonly struct SectionId
{
    public string Value { get; }
    public SectionId(string value) => Value = value;
    public override string ToString() => Value;
}
