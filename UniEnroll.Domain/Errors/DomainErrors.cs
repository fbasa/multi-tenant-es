
namespace UniEnroll.Domain.Errors;

public static class DomainErrors
{
    public static string CapacityFull => "No remaining capacity.";
    public static string WaitlistFull => "Waitlist is full.";
    public static string InvalidPrerequisites => "Prerequisites not satisfied.";
    public static string EnrollmentWindowClosed => "Enrollment window is closed.";
}
