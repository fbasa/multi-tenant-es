
using System.Security.Cryptography;

namespace UniEnroll.Infrastructure.Common.Security;

public static class PasswordHasher
{
    public static (byte[] Hash, byte[] Salt) Hash(string password, int iterations = 100_000)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        return (pbkdf2.GetBytes(32), salt);
    }

    public static bool Verify(string password, byte[] hash, byte[] salt, int iterations = 100_000)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        var candidate = pbkdf2.GetBytes(32);
        return CryptographicOperations.FixedTimeEquals(candidate, hash);
    }
}
