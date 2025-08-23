
using System;
using System.Linq;

namespace UniEnroll.Infrastructure.EF.Persistence.Repositories;

public static class SpecificationEvaluator
{
    public static IQueryable<T> Apply<T>(IQueryable<T> query) => query; // Placeholder for spec pattern wiring.
}
