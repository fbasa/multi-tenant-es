
namespace UniEnroll.Domain.Grades;

public readonly struct GradeId
{
    public string Value { get; }
    public GradeId(string value) => Value = value;
    public override string ToString() => Value;
}
