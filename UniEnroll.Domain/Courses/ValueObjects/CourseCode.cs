
namespace UniEnroll.Domain.Courses.ValueObjects;

public readonly struct CourseCode
{
    public string Value { get; }
    public CourseCode(string value) => Value = value;
    public override string ToString() => Value;
}
