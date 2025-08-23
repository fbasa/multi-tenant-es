
using System;
using System.Linq.Expressions;
using UniEnroll.Domain.Courses;

namespace UniEnroll.Domain.Specifications;

public static class ActiveCoursesSpec
{
    public static Expression<Func<Course, bool>> Build()
        => c => true; // placeholder; add status if available
}
