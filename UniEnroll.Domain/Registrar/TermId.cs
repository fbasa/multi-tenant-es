
namespace UniEnroll.Domain.Registrar;

public readonly struct TermId
{
    public string Value { get; }
    public TermId(string value) => Value = value;
    public override string ToString() => Value;
}
