
using UniEnroll.Domain.Scheduling.ValueObjects;

namespace UniEnroll.Domain.Scheduling.Constraints;

public static class NoInstructorClash
{
    public static bool Violates(string instructorId, DayOfWeekVO day, TimeRange a, TimeRange b)
        => a.Start < b.End && b.Start < a.End;
}
