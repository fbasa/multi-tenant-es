
using System.Collections.Generic;

namespace UniEnroll.Contracts.Common;

public sealed class PagedResponse<T>
{
    public int Page { get; init; }
    public int Size { get; init; }
    public int Total { get; init; }
    public IReadOnlyList<T> Items { get; init; } = new List<T>();
}
