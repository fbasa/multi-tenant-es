
namespace UniEnroll.Api.Auth;

public static class Policies
{
    public static class Student
    {
        public const string Read = "student.read";
        public const string WriteProfile = "student.profile.write";
        public const string Enroll = "student.enroll";
        public const string UploadRequirements = "student.requirements.upload";
        public const string RequestTranscript = "student.transcript.request";
        public const string ViewTranscript = "student.transcript.view";   // <-- add this
        public const string ViewLedger = "student.ledger.view";
        public const string Pay = "student.pay";
    }
    public static class Instructor
    {
        public const string ViewLoads = "instructor.loads.read";
        public const string ManageSections = "instructor.sections.manage";
        public const string RecordGrades = "instructor.grades.record";
    }

    public static class Registrar
    {
        public const string FullAccess = "registrar.full";
        public const string ManageTenants = "registrar.tenants.manage";
        public const string ManageTerms = "registrar.terms.manage";
        public const string PublishCatalog = "registrar.catalog.publish";
        public const string ManageCatalog = "registrar.catalog.manage";           // used by CoursesController
        public const string ManageEnrollmentWindows = "registrar.enrollmentwindows.manage";
        public const string ManageHolds = "registrar.holds.manage";
        public const string ApproveGrades = "registrar.grades.approve";
        public const string RunGraduationAudit = "registrar.graduation.audit.run";
        public const string ManageBilling = "registrar.billing.manage";
        public const string ManageInstructors = "registrar.instructors.manage";
        public const string OptimizeSchedule = "registrar.schedule.optimize";
    }

    public static class Reporting
    {
        public const string View = "report.view";
        public const string Export = "report.export";
    }

    public static class Billing
    {
        public const string View    = "billing:view";
        public const string Capture = "billing:capture";
        public const string Refund  = "billing:refund";
    }

    public static class Scheduling
    {
        public const string Build             = "scheduling:build";
        public const string AssignRoom        = "scheduling:assign-room";
        public const string Optimize          = "scheduling:optimize";
        public const string ViewConflicts     = "scheduling:view-conflicts";
        public const string ViewStudentSchedule= "scheduling:view-student-schedule";
    }
}
