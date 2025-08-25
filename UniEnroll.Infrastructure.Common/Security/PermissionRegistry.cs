
namespace UniEnroll.Infrastructure.Common.Security;

public static class PermissionRegistry
{
    // Central list of all known permissions used by policies
    public static readonly string[] All = Groups.AllGroups.SelectMany(g => g).Distinct().OrderBy(s => s).ToArray();

    public static bool IsKnown(string permission) => All.Contains(permission);

    public static class Groups
    {
        // Student
        public static readonly string[] StudentViewTranscript = { "student:view-transcript" };
        public static readonly string[] StudentEnroll         = { "student:enroll" };
        public static readonly string[] StudentPortal         = { "student:portal" };

        // Instructor
        public static readonly string[] InstructorView   = { "instructor:view" };
        public static readonly string[] InstructorManage = { "instructor:manage" };
        public static readonly string[] InstructorAssign = { "instructor:assign" };

        // Registrar
        public static readonly string[] RegistrarView   = { "registrar:view" };
        public static readonly string[] RegistrarManage = { "registrar:manage" };

        // Reporting
        public static readonly string[] ReportingView   = { "reporting:view" };
        public static readonly string[] ReportingExport = { "reporting:export" };

        // Billing 
        public static readonly string[] BillingView     = { "billing:view" };
        public static readonly string[] BillingCapture  = { "billing:capture" };
        public static readonly string[] BillingRefund   = { "billing:refund" };

        // Scheduling
        public static readonly string[] SchedulingBuild              = { "scheduling:build" };
        public static readonly string[] SchedulingAssignRoom         = { "scheduling:assign-room" };
        public static readonly string[] SchedulingOptimize           = { "scheduling:optimize" };
        public static readonly string[] SchedulingViewConflicts      = { "scheduling:view-conflicts" };
        public static readonly string[] SchedulingViewStudentSchedule= { "scheduling:view-student-schedule" };

        // Helper to enumerate all groups
        public static IEnumerable<string[]> AllGroups => new[]
        {
            StudentViewTranscript, StudentEnroll, StudentPortal,
            InstructorView, InstructorManage, InstructorAssign,
            RegistrarView, RegistrarManage,
            ReportingView, ReportingExport,
            BillingView, BillingCapture, BillingRefund,
            SchedulingBuild, SchedulingAssignRoom, SchedulingOptimize, SchedulingViewConflicts, SchedulingViewStudentSchedule
        };
    }
}
