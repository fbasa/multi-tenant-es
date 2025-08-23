
namespace UniEnroll.Domain.Courses;

public readonly struct CourseId
{
    public string Value { get; }
    public CourseId(string value) => Value = value;
    public override string ToString() => Value;
}
