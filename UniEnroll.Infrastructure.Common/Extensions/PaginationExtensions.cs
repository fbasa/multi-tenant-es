
using System.Linq;

namespace UniEnroll.Infrastructure.Common.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> Page<T>(this IQueryable<T> q, int page, int size)
        => q.Skip((page - 1) * size).Take(size);
}
