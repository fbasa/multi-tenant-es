
using System.Collections.Generic;

namespace UniEnroll.Application.Common;

public sealed class PagedResult<T>
{
    public int Page { get; init; }
    public int Size { get; init; }
    public long Total { get; init; }
    public IReadOnlyList<T> Items { get; init; }

    public PagedResult(int page, int size, long total, IReadOnlyList<T> items)
    {
        Page = page; Size = size; Total = total; Items = items;
    }
}
