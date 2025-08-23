
namespace UniEnroll.Api.Auth;

public static class Permissions
{
    public static class Student
    {
        public const string Read = "student.read";
        public const string Write = "student.write";
    }
    public static class Instructor
    {
        public const string Read = "instructor.read";
        public const string Write = "instructor.write";
    }
    public static class Registrar
    {
        public const string Read = "registrar.read";
    }
    public static class Reporting
    {
        public const string Read = "report.read";
    }
}
