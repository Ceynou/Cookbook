using Cookbook.Core;
using Xunit;

namespace Cookbook.API.UnitTests;

public class PasswordHasherTests
{
    private readonly IPasswordHasher _passwordHasher = new PasswordHasher();

    #region Integration Tests

    [Fact]
    public void PasswordHasher_RoundTripTest_WorksCorrectly()
    {
        // Arrange
        var passwords = new[]
        {
            "SimplePassword",
            "C0mpl3x!P@ssw0rd",
            "123456",
            "password with spaces",
            "UPPERCASE",
            "lowercase",
            "MiXeDcAsE123"
        };

        foreach (var password in passwords)
        {
            // Act
            var hashed = _passwordHasher.HashPassword(password);
            var verified = _passwordHasher.VerifyPassword(password, hashed);

            // Assert
            Assert.True(verified, $"Failed to verify password: {password}");
        }
    }

    #endregion

    #region HashPassword Tests

    [Fact]
    public void HashPassword_WithValidPassword_ReturnsHashedPassword()
    {
        // Arrange
        const string password = "MySecurePassword123!";

        // Act
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEmpty(hashedPassword);
        Assert.NotEqual(password, hashedPassword); // Hash should be different from plain password
    }

    [Fact]
    public void HashPassword_WithValidPassword_ReturnsCorrectFormat()
    {
        // Arrange
        const string password = "TestPassword123";

        // Act
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Assert
        // Format should be: &lt;hash&gt;:&lt;salt&gt; where both are hex strings
        Assert.Contains(":", hashedPassword);
        var parts = hashedPassword.Split(':');
        Assert.Equal(2, parts.Length);

        // Verify both parts are valid hex strings
        Assert.Matches("^[0-9A-F]+$", parts[0]); // Hash part
        Assert.Matches("^[0-9A-F]+$", parts[1]); // Salt part
    }

    [Fact]
    public void HashPassword_CalledTwiceWithSamePassword_ReturnsDifferentHashes()
    {
        // Arrange
        const string password = "SamePassword123";

        // Act
        var hash1 = _passwordHasher.HashPassword(password);
        var hash2 = _passwordHasher.HashPassword(password);

        // Assert
        Assert.NotEqual(hash1, hash2); // Different salts should produce different hashes
    }

    [Fact]
    public void HashPassword_WithEmptyPassword_ReturnsHash()
    {
        // Arrange
        const string password = "";

        // Act
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.Contains(":", hashedPassword);
    }

    [Theory]
    [InlineData("password123")]
    [InlineData("P@ssw0rd!")]
    [InlineData("very_long_password_with_many_characters_1234567890")]
    [InlineData("短密码")]
    [InlineData("пароль")]
    public void HashPassword_WithVariousPasswords_ReturnsValidHash(string password)
    {
        // Act
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.Contains(":", hashedPassword);
        var parts = hashedPassword.Split(':');
        Assert.Equal(2, parts.Length);
    }

    #endregion

    #region VerifyPassword Tests

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
    {
        // Arrange
        const string password = "MySecurePassword123!";
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyPassword(password, hashedPassword);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ReturnsFalse()
    {
        // Arrange
        const string correctPassword = "CorrectPassword123";
        const string incorrectPassword = "WrongPassword456";
        var hashedPassword = _passwordHasher.HashPassword(correctPassword);

        // Act
        var result = _passwordHasher.VerifyPassword(incorrectPassword, hashedPassword);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_WithCaseSensitivePassword_ReturnsFalse()
    {
        // Arrange
        const string password = "Password123";
        const string differentCasePassword = "password123";
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyPassword(differentCasePassword, hashedPassword);

        // Assert
        Assert.False(result); // Passwords are case-sensitive
    }

    [Fact]
    public void VerifyPassword_WithEmptyPassword_VerifiesCorrectly()
    {
        // Arrange
        const string password = "";
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Act
        var correctResult = _passwordHasher.VerifyPassword("", hashedPassword);
        var incorrectResult = _passwordHasher.VerifyPassword("notEmpty", hashedPassword);

        // Assert
        Assert.True(correctResult);
        Assert.False(incorrectResult);
    }

    [Theory]
    [InlineData("password123")]
    [InlineData("P@ssw0rd!")]
    [InlineData("very_long_password_with_many_characters_1234567890")]
    [InlineData("短密码")]
    [InlineData("пароль")]
    public void VerifyPassword_WithVariousPasswords_VerifiesCorrectly(string password)
    {
        // Arrange
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Act
        var correctResult = _passwordHasher.VerifyPassword(password, hashedPassword);
        var incorrectResult = _passwordHasher.VerifyPassword(password + "wrong", hashedPassword);

        // Assert
        Assert.True(correctResult);
        Assert.False(incorrectResult);
    }

    [Fact]
    public void VerifyPassword_WithSlightlyDifferentPassword_ReturnsFalse()
    {
        // Arrange
        const string password = "Password123";
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Act & Assert
        Assert.False(_passwordHasher.VerifyPassword("Password123 ", hashedPassword)); // Extra space
        Assert.False(_passwordHasher.VerifyPassword(" Password123", hashedPassword)); // Leading space
        Assert.False(_passwordHasher.VerifyPassword("Password12", hashedPassword)); // Missing character
        Assert.False(_passwordHasher.VerifyPassword("Password1234", hashedPassword)); // Extra character
    }

    #endregion

    #region Hash Security Tests

    [Fact]
    public void HashPassword_GeneratesUniqueSalts()
    {
        // Arrange
        const string password = "TestPassword";
        var hashes = new List<string>();

        // Act - Generate 10 hashes of the same password
        for (var i = 0; i < 10; i++) hashes.Add(_passwordHasher.HashPassword(password));

        // Assert - All hashes should be unique (different salts)
        var uniqueHashes = hashes.Distinct().Count();
        Assert.Equal(10, uniqueHashes);
    }

    [Fact]
    public void VerifyPassword_MultipleHashesOfSamePassword_AllVerifyCorrectly()
    {
        // Arrange
        const string password = "SamePassword";
        var hash1 = _passwordHasher.HashPassword(password);
        var hash2 = _passwordHasher.HashPassword(password);
        var hash3 = _passwordHasher.HashPassword(password);

        // Act & Assert - All different hashes should verify the same password
        Assert.True(_passwordHasher.VerifyPassword(password, hash1));
        Assert.True(_passwordHasher.VerifyPassword(password, hash2));
        Assert.True(_passwordHasher.VerifyPassword(password, hash3));

        // But they should be different hashes
        Assert.NotEqual(hash1, hash2);
        Assert.NotEqual(hash2, hash3);
        Assert.NotEqual(hash1, hash3);
    }

    [Fact]
    public void HashPassword_ProducesConsistentLength()
    {
        // Arrange & Act
        var hash1 = _passwordHasher.HashPassword("short");
        var hash2 = _passwordHasher.HashPassword("this_is_a_much_longer_password");
        var hash3 = _passwordHasher.HashPassword("MediumPassword123");

        // Assert - Hash and salt parts should have consistent lengths
        var parts1 = hash1.Split(':');
        var parts2 = hash2.Split(':');
        var parts3 = hash3.Split(':');

        Assert.Equal(parts1[0].Length, parts2[0].Length); // Hash length should be the same
        Assert.Equal(parts2[0].Length, parts3[0].Length);

        Assert.Equal(parts1[1].Length, parts2[1].Length); // Salt length should be the same
        Assert.Equal(parts2[1].Length, parts3[1].Length);
    }

    #endregion
}