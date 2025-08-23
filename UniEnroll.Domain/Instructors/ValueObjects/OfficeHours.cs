
using System;

namespace UniEnroll.Domain.Instructors.ValueObjects;

public sealed class OfficeHours
{
    public DayOfWeek Day { get; }
    public TimeSpan From { get; }
    public TimeSpan To { get; }

    public OfficeHours(DayOfWeek day, TimeSpan from, TimeSpan to)
    {
        if (to <= from) throw new ArgumentException("To must be after From");
        Day = day; From = from; To = to;
    }
}
