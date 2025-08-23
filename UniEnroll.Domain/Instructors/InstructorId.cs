
namespace UniEnroll.Domain.Instructors;

public readonly struct InstructorId
{
    public string Value { get; }
    public InstructorId(string value) => Value = value;
    public override string ToString() => Value;
}
