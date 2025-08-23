
namespace UniEnroll.Domain.Students;

public readonly struct StudentId
{
    public string Value { get; }
    public StudentId(string value) => Value = value;
    public override string ToString() => Value;
}
