
using System;
using System.Linq.Expressions;
using UniEnroll.Domain.Students;

namespace UniEnroll.Domain.Specifications;

public static class StudentByEmailSpec
{
    public static Expression<Func<Student, bool>> Build(string email)
        => s => s.Email == email;
}
