
namespace UniEnroll.Domain.Instructors.ValueObjects;

public readonly struct TeachingLoad
{
    public int Units { get; }
    public TeachingLoad(int units) { Units = units < 0 ? 0 : units; }
}
