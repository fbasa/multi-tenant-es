
namespace UniEnroll.Domain.Courses.ValueObjects;

public readonly struct CreditUnit
{
    public int Value { get; }
    public CreditUnit(int value) => Value = value;
    public override string ToString() => Value.ToString();
}
