
using System;
using System.Linq.Expressions;
using UniEnroll.Domain.Enrollment;

namespace UniEnroll.Domain.Specifications;

public static class EnrollmentByStudentTermSpec
{
    public static Expression<Func<Enrollment.Enrollment, bool>> Build(string studentId, string termId)
        => e => e.StudentId == studentId; // Section.TermId join left to repo
}
