
using System.Collections.Generic;

namespace UniEnroll.Application.Common.Pagination;

public sealed class KeysetPageResult<T>
{
    public string? Next { get; init; }
    public string? Prev { get; init; }
    public int PageSize { get; init; }
    public IReadOnlyList<T> Items { get; init; }

    public KeysetPageResult(string? next, string? prev, int pageSize, IReadOnlyList<T> items)
    {
        Next = next; Prev = prev; PageSize = pageSize; Items = items;
    }
}
