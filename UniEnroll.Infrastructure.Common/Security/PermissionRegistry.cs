
namespace UniEnroll.Infrastructure.Common.Security;

public sealed class PermissionRegistry
{
    public static readonly string[] All = new[]
    {
        "student.read","student.write",
        "instructor.read","instructor.write",
        "registrar.read",
        "report.read"
    };
}
