
namespace UniEnroll.Application.Common.Policies;

public static class StudentOwnershipGuard
{
    /// <summary>Checks that the caller operates on their own studentId unless privileged.</summary>
    public static bool IsOwnerOrPrivileged(string callerUserId, string studentUserId, string[] roles)
        => callerUserId == studentUserId || roles.Contains("Admin") || roles.Contains("Registrar");
}
