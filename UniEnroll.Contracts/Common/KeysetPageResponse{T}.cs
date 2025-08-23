
using System.Collections.Generic;

namespace UniEnroll.Contracts.Common;

public sealed class KeysetPageResponse<T>
{
    public string? Next { get; init; }
    public string? Prev { get; init; }
    public int PageSize { get; init; }
    public IReadOnlyList<T> Items { get; init; } = new List<T>();
}
