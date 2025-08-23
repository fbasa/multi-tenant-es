
using System;
using System.Collections.Generic;
using System.Linq;

namespace UniEnroll.Application.Common.Pagination;

public static class KeysetPagination
{
    /// <summary>
    /// Performs simple keyset pagination on an ordered enumerable given a key selector.
    /// </summary>
    public static KeysetPageResult<T> PageForward<T, TKey>(IEnumerable<T> source, int size, Func<T, TKey> keySelector, string? afterToken = null)
        where TKey : IComparable<TKey>
    {
        var list = source.ToList();
        if (!string.IsNullOrWhiteSpace(afterToken) && KeysetPageToken.TryDecode(afterToken, out var afterKeyStr) && afterKeyStr is not null)
        {
            list = list.SkipWhile(i => keySelector(i)?.ToString() != afterKeyStr).Skip(1).ToList();
        }
        var pageItems = list.Take(size).ToList();
        var next = pageItems.Count == size ? KeysetPageToken.Encode(keySelector(pageItems.Last())?.ToString() ?? "") : null;
        var prev = pageItems.Count > 0 ? KeysetPageToken.Encode(keySelector(pageItems.First())?.ToString() ?? "") : null;
        return new KeysetPageResult<T>(next, prev, size, pageItems);
    }
}
