
namespace UniEnroll.Domain.Grades.ValueObjects;

public readonly struct GradeValue
{
    public decimal Points { get; }
    public GradeValue(decimal points) { Points = points; }
    public override string ToString() => Points.ToString("0.00");
}
