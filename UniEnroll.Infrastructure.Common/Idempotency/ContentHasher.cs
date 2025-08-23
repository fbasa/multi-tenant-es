using System.Security.Cryptography;
using System.Text;

namespace UniEnroll.Infrastructure.Common.Idempotency;

public static class ContentHasher
{
    public static string Sha256(string content)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(content ?? string.Empty));
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes) sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}
