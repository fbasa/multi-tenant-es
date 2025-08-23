
using System;

namespace UniEnroll.Domain.Scheduling.ValueObjects;

public readonly struct TimeRange
{
    public TimeSpan Start { get; }
    public TimeSpan End { get; }
    public TimeRange(TimeSpan start, TimeSpan end)
    {
        if (end <= start) throw new System.ArgumentException("End must be after Start");
        Start = start; End = end;
    }
}
