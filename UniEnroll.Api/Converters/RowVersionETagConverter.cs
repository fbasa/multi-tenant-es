
using System;

namespace UniEnroll.Api.Converters;

public static class RowVersionETagConverter
{
    public static string ToETag(byte[] rowVersion)
    {
        var str = Convert.ToBase64String(rowVersion);
        return rowVersion is null ? string.Empty : $"W/" + str;
    }

    public static bool TryParse(string? etag, out byte[]? rowVersion)
    {
        rowVersion = null;
        if (string.IsNullOrWhiteSpace(etag)) return false;
        var s = etag.Trim().TrimStart('W').TrimStart('/').Trim('"');
        try { rowVersion = Convert.FromBase64String(s); return true; }
        catch { return false; }
    }
}
