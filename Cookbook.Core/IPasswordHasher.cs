namespace Cookbook.Core;

/// <summary>
///     Provides functionality for securely hashing and verifying passwords using PBKDF2 with SHA-512.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    ///     Hashes the specified plaintext password using PBKDF2 with a randomly generated salt.
    /// </summary>
    /// <param name="password">The plaintext password to hash.</param>
    /// <returns>
    ///     A string containing the hexadecimal representation of the hash and salt, separated by a delimiter.
    /// </returns>
    /// <remarks>
    ///     The output format is: &lt;hash&gt;:&lt;salt&gt;, where both values are hex-encoded.
    /// </remarks>
    string HashPassword(string password);

    /// <summary>
    ///     Verifies whether the provided plaintext password matches the previously hashed password.
    /// </summary>
    /// <param name="providedPassword">The plaintext password to verify.</param>
    /// <param name="hashedPassword">The previously hashed password string to compare against.</param>
    /// <returns>
    ///     <c>true</c> if the provided password matches the hashed password; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     This method uses constant-time comparison to prevent timing attacks.
    /// </remarks>
    bool VerifyPassword(string providedPassword, string hashedPassword);
}