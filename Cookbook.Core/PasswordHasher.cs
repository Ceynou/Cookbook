using System.Security.Cryptography;

namespace Cookbook.Core;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 8;
    private const int HashSize = 16;
    private const int Iterations = 100000;
    private const char Delimiter = ':';

    private readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA512;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, HashSize);
        return $"{Convert.ToHexString(hash)}{Delimiter}{Convert.ToHexString(salt)}";
    }

    public bool VerifyPassword(string providedPassword, string hashedPassword)
    {
        var parts = hashedPassword.Split(Delimiter);
        var hash = Convert.FromHexString(parts[0]);
        var salt = Convert.FromHexString(parts[1]);

        var providedPasswordHash =
            Rfc2898DeriveBytes.Pbkdf2(providedPassword, salt, Iterations, _hashAlgorithmName, HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, providedPasswordHash);
    }
}