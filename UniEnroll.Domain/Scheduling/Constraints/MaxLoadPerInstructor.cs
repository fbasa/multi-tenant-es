
namespace UniEnroll.Domain.Scheduling.Constraints;

public static class MaxLoadPerInstructor
{
    public static bool Exceeds(int currentUnits, int addingUnits, int maxUnits) => currentUnits + addingUnits > maxUnits;
}
