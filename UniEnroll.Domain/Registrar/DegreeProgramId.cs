
namespace UniEnroll.Domain.Registrar;

public readonly struct DegreeProgramId
{
    public string Value { get; }
    public DegreeProgramId(string value) => Value = value;
    public override string ToString() => Value;
}
