
namespace UniEnroll.Domain.Enrollment.Policies;

public static class SeatAllocationPolicy
{
    public static bool CanEnroll(int capacity, int seatsTaken) => seatsTaken < capacity;
    public static bool CanWaitlist(int waitlistCapacity, int waitlistCount) => waitlistCount < waitlistCapacity;
}
