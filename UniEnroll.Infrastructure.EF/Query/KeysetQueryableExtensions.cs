
using System;
using System.Linq;

namespace UniEnroll.Infrastructure.EF.Query;

public static class KeysetQueryableExtensions
{
    public static IQueryable<T> WhereAfter<T, TKey>(this IQueryable<T> q, Func<T, TKey> keySelector, TKey afterKey) => q;
}
