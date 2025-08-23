
using System;
using UniEnroll.Domain.Scheduling.ValueObjects;

namespace UniEnroll.Domain.Scheduling.Constraints;

public static class NoRoomClash
{
    public static bool Violates(string room, DayOfWeekVO day, TimeRange a, TimeRange b)
        => room is not null && a.Start < b.End && b.Start < a.End;
}
