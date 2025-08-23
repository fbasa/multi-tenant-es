
namespace UniEnroll.Domain.Enrollment;

public readonly struct EnrollmentId
{
    public string Value { get; }
    public EnrollmentId(string value) => Value = value;
    public override string ToString() => Value;
}
