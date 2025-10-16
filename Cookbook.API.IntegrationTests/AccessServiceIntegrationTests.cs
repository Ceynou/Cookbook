using Cookbook.API.IntegrationTests.Fixtures;
using Cookbook.Core;
using Cookbook.SharedData.Entities;
using Cookbook.SharedData.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cookbook.API.IntegrationTests;

public class AccessServiceIntegrationTests : IntegrationTest
{
    private readonly IAccessService _accessService;
    private readonly IPasswordHasher _passwordHasher;

    public AccessServiceIntegrationTests(APiWebApplicationFactory webApi) : base(webApi)
    {
        var scope = webApi.Services.CreateScope();
        _accessService = scope.ServiceProvider.GetRequiredService<IAccessService>();
        _passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    }

    [Fact]
    public async Task SignUpAsync_WithValidUser_CreatesUserSuccessfully()
    {
        // Arrange
        var newUser = new User
        {
            Username = "signuptest1",
            Email = "signuptest1@example.com",
            PasswordHash = _passwordHasher.HashPassword("TestPass123"),
            IsAdmin = false
        };

        // Act
        var result = await _accessService.SignUpAsync(newUser);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.UserId > 0);
        Assert.Equal(newUser.Username, result.Username);
        Assert.Equal(newUser.Email, result.Email);
        Assert.False(result.IsAdmin);
    }

    [Fact]
    public async Task SignUpAsync_WithDuplicateUsername_ThrowsDuplicatePropertyException()
    {
        // Arrange
        var newUser = new User
        {
            Username = "user", // Already exists in seed data
            Email = "uniqueemail@example.com",
            PasswordHash = _passwordHasher.HashPassword("TestPass123"),
            IsAdmin = false
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DuplicatePropertyException>(() => _accessService.SignUpAsync(newUser));

        Assert.Contains("Username", exception.Message);
    }

    [Fact]
    public async Task SignUpAsync_WithDuplicateEmail_ThrowsDuplicatePropertyException()
    {
        // Arrange
        var newUser = new User
        {
            Username = "uniqueusername",
            Email = "user@user.com", // Already exists in seed data
            PasswordHash = _passwordHasher.HashPassword("TestPass123"),
            IsAdmin = false
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DuplicatePropertyException>(() => _accessService.SignUpAsync(newUser));

        Assert.Contains("Email", exception.Message);
    }

    [Fact]
    public async Task SignUpAsync_WithAdminFlag_CreatesAdminUser()
    {
        // Arrange
        var newUser = new User
        {
            Username = "admintest1",
            Email = "admintest1@example.com",
            PasswordHash = _passwordHasher.HashPassword("AdminPass123"),
            IsAdmin = true
        };

        // Act
        var result = await _accessService.SignUpAsync(newUser);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsAdmin);
    }

    [Fact]
    public async Task SignInAsync_WithValidCredentials_ReturnsUser()
    {
        // Arrange
        var signInUser = new User
        {
            Username = "user",
            PasswordHash = "user" // Plain password for verification
        };

        // Act
        var result = await _accessService.SignInAsync(signInUser);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("user", result.Username);
        Assert.Equal("user@user.com", result.Email);
    }

    [Fact]
    public async Task SignInAsync_WithInvalidUsername_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var signInUser = new User
        {
            Username = "nonexistentuser",
            PasswordHash = "Password123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _accessService.SignInAsync(signInUser));
    }

    [Fact]
    public async Task SignInAsync_WithInvalidPassword_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var signInUser = new User
        {
            Username = "testuser",
            PasswordHash = "WrongPassword123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _accessService.SignInAsync(signInUser));
    }

    [Fact]
    public async Task SignInAsync_WithAdminUser_ReturnsUserWithAdminFlag()
    {
        // Arrange
        var signInUser = new User
        {
            Username = "admin",
            PasswordHash = "admin"
        };

        // Act
        var result = await _accessService.SignInAsync(signInUser);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsAdmin);
        Assert.Equal("admin", result.Username);
    }

    [Fact]
    public async Task SignUpAsync_ThenSignInAsync_WorksCorrectly()
    {
        // Arrange
        const string password = "IntegrationTest123";
        const string username = "integrationuser2";
        var newUser = new User
        {
            Username = username,
            Email = "integrationuser2@example.com",
            PasswordHash = password,
            IsAdmin = false
        };

        // Act - Sign up
        var signUpResult = await _accessService.SignUpAsync(newUser);
        Assert.NotNull(signUpResult);

        // Act - Sign in
        var signInUser = new User
        {
            Username = username,
            PasswordHash = password
        };
        var signInResult = await _accessService.SignInAsync(signInUser);

        // Assert
        Assert.NotNull(signInResult);
        Assert.Equal(signUpResult.UserId, signInResult.UserId);
        Assert.Equal(signUpResult.Username, signInResult.Username);
        Assert.Equal(signUpResult.Email, signInResult.Email);
    }
}