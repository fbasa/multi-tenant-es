using Microsoft.EntityFrameworkCore;
using UniEnroll.Domain.Courses;
using UniEnroll.Domain.Enrollment;
using UniEnroll.Domain.Students;

namespace UniEnroll.Infrastructure.EF.Persistence;

public static class CompiledQueries
{
    public static readonly Func<UniEnrollDbContext, string, Task<Student?>> StudentById =
        Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((UniEnrollDbContext db, string id) =>
            db.Students.AsNoTracking()
                .FirstOrDefault(s => s.Id == id));

    public static readonly Func<UniEnrollDbContext, string, Task<Course?>> CourseByCode =
        Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((UniEnrollDbContext db, string courseCode) =>
            db.Courses.AsNoTracking()
              .Where(c => c.Code.Value == courseCode)
              .FirstOrDefault());

    public static readonly Func<UniEnrollDbContext, string, Task<List<Course>>> CourseLists =
        Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((UniEnrollDbContext db, string courseCode) =>
            db.Courses.AsNoTracking()
              .Where(p => p.Code.Value == courseCode)
              .ToList());

    public static readonly Func<UniEnrollDbContext, string, Task<List<Enrollment>>> EnrollmentsByStudent = 
        Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((UniEnrollDbContext db, string studentId) => 
            db.Enrollments.AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .ToList());
}
