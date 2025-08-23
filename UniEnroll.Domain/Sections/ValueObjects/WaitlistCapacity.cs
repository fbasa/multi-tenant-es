
namespace UniEnroll.Domain.Sections.ValueObjects;

public readonly struct WaitlistCapacity
{
    public int Value { get; }
    public WaitlistCapacity(int value) { Value = value < 0 ? 0 : value; }
}
