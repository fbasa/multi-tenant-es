
using System;
using System.Linq;
using System.Linq.Expressions;

namespace UniEnroll.Infrastructure.Common.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        => condition ? query.Where(predicate) : query;
}
