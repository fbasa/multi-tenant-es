
using System.Security.Cryptography;

namespace UniEnroll.Infrastructure.Common.Security;

public static class TokenHasher
{
    /// <summary>Compute SHA256 over the raw token bytes.</summary>
    public static byte[] Sha256(byte[] token)
    {
        using var sha = SHA256.Create();
        return sha.ComputeHash(token);
    }

    public static byte[] Sha256(string token) => Sha256(System.Text.Encoding.UTF8.GetBytes(token));

    /// <summary>Create a cryptographically-strong random token (base64url).</summary>
    public static string CreateSecureToken(int numBytes = 32)
    {
        var bytes = RandomNumberGenerator.GetBytes(numBytes);
        return Base64UrlEncode(bytes);
    }

    public static string Base64UrlEncode(byte[] bytes)
    {
        return System.Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
}
