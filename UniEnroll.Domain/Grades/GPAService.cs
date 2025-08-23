
using System.Collections.Generic;
using System.Linq;

namespace UniEnroll.Domain.Grades;

public static class GPAService
{
    public static decimal Compute(IReadOnlyCollection<(int units, decimal points)> grades)
    {
        if (grades.Count == 0) return 0;
        var w = grades.Sum(g => g.units * g.points);
        var u = grades.Sum(g => g.units);
        return u == 0 ? 0 : w / u;
    }
}
