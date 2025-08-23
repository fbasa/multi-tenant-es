
using System;
using System.Text;

namespace UniEnroll.Application.Common.Pagination;

/// <summary>Opaque keyset token encoder/decoder (base64 JSON of last key).</summary>
public static class KeysetPageToken
{
    public static string Encode(string lastKey)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(lastKey));

    public static bool TryDecode(string? token, out string? lastKey)
    {
        lastKey = null;
        if (string.IsNullOrWhiteSpace(token)) return false;
        try { lastKey = Encoding.UTF8.GetString(Convert.FromBase64String(token)); return true; }
        catch { return false; }
    }
}
