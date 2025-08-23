
namespace UniEnroll.Domain.Sections.ValueObjects;

public readonly struct Capacity
{
    public int Total { get; }
    public int Waitlist { get; }
    public Capacity(int total, int waitlist) { Total = total; Waitlist = waitlist; }
}
