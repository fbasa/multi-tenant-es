
namespace UniEnroll.Domain.Registrar.Services;

public static class GraduationPolicy
{
    public static bool IsEligible(int totalUnits, int requiredUnits) => totalUnits >= requiredUnits;
}
