using Cookbook.Core;
using Cookbook.Data.Repositories;
using Cookbook.SharedData.Entities;
using Moq;
using Xunit;

namespace Cookbook.API.Tests;

public class AccessServiceUnitTests
{
    // Mock Data
    private const string RawPassword = "Password123";
    private const string HashedPassword = "HASHED_PASSWORD_VALUE";

    private readonly User _mockDbUser = new()
    {
        UserId = 1,
        Username = "testuser",
        PasswordHash = HashedPassword,
        Email = "user01@live.fr",
        ImagePath = "TestImage.jpg",
        IsAdmin = false
    };

    private readonly User _mockInputUser = new()
    {
        Username = "testuser",
        PasswordHash = RawPassword
    };

    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly AccessService _sut;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public AccessServiceUnitTests()
    {
        // Use MockBehavior.Strict to ensure all dependencies are explicitly called
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _passwordHasherMock = new Mock<IPasswordHasher>(MockBehavior.Strict);
        _sut = new AccessService(_userRepositoryMock.Object, _passwordHasherMock.Object);

        // Setup password hashing for sign up (always successful)
        _passwordHasherMock
            .Setup(h => h.HashPassword(RawPassword))
            .Returns(HashedPassword);
    }

    #region SignUpAsync Tests

    [Fact]
    public async Task SignUpAsync_WhenSuccessful_Should_HashPassword_CallRepositoryCreate_And_ReturnCreatedUser()
    {
        // ARRANGE
        // 1. Setup repository mock to return the created user (with ID and hashed password)
        _userRepositoryMock
            .Setup(r => r.CreateAsync(
                It.Is<User>(u => u.PasswordHash == HashedPassword))) // Ensure hashed password is passed to repo
            .ReturnsAsync(_mockDbUser);

        // ACT
        var actualUser = await _sut.SignUpAsync(_mockInputUser);

        // ASSERT
        Assert.Equal(_mockDbUser.UserId, actualUser.UserId);
        Assert.Equal(HashedPassword, actualUser.PasswordHash);

        // Verify interactions
        _passwordHasherMock.Verify(h => h.HashPassword(RawPassword), Times.Once); //
        _userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once); //
    }

    [Fact]
    public async Task SignUpAsync_WhenRepositoryReturnsNull_Should_ThrowException()
    {
        // ARRANGE
        // 1. Setup repository mock to return null (simulating a creation failure, e.g., unique constraint violation)
        _userRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((User)null!);

        // ACT & ASSERT
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.SignUpAsync(_mockInputUser));

        // Verify the expected exception message is thrown
        Assert.Equal("Could not create user", exception.Message);

        // Verify interactions
        _passwordHasherMock.Verify(h => h.HashPassword(RawPassword), Times.Once);
        _userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    #endregion

    #region SignInAsync Tests

    [Fact]
    public async Task SignInAsync_WithValidCredentials_Should_ReturnDbUser()
    {
        // ARRANGE
        // 1. Setup repository mock to return the existing user
        _userRepositoryMock
            .Setup(r => r.GetByUsernameAsync(_mockInputUser.Username))
            .ReturnsAsync(_mockDbUser); //

        // 2. Setup password hasher mock to return success
        _passwordHasherMock
            .Setup(h => h.VerifyPassword(RawPassword, HashedPassword))
            .Returns(true); //

        // ACT
        var actualUser = await _sut.SignInAsync(_mockInputUser);

        // ASSERT
        Assert.Equal(_mockDbUser.UserId, actualUser.UserId);
        Assert.Equal(_mockDbUser.Username, actualUser.Username);

        // Verify interactions
        _userRepositoryMock.Verify(r => r.GetByUsernameAsync(_mockInputUser.Username), Times.Once);
        _passwordHasherMock.Verify(h => h.VerifyPassword(RawPassword, HashedPassword), Times.Once);
    }

    [Fact]
    public async Task SignInAsync_WhenUserNotFound_Should_ThrowInvalidCredentialsException()
    {
        // ARRANGE
        // 1. Setup repository mock to return null
        _userRepositoryMock
            .Setup(r => r.GetByUsernameAsync(_mockInputUser.Username))
            .ReturnsAsync((User)null!); //

        // ACT & ASSERT
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.SignInAsync(_mockInputUser));

        // Verify the expected exception message is thrown
        Assert.Equal("Invalid credentials", exception.Message);

        // Verify interactions
        _userRepositoryMock.Verify(r => r.GetByUsernameAsync(_mockInputUser.Username), Times.Once);
        // Password hasher should NOT be called
        _passwordHasherMock.Verify(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SignInAsync_WithIncorrectPassword_Should_ThrowInvalidCredentialsException()
    {
        // ARRANGE
        // 1. Setup repository mock to return the existing user
        _userRepositoryMock
            .Setup(r => r.GetByUsernameAsync(_mockInputUser.Username))
            .ReturnsAsync(_mockDbUser);

        // 2. Setup password hasher mock to return failure
        _passwordHasherMock
            .Setup(h => h.VerifyPassword(RawPassword, HashedPassword))
            .Returns(false); //

        // ACT & ASSERT
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.SignInAsync(_mockInputUser));

        // Verify the expected exception message is thrown
        Assert.Equal("Invalid credentials", exception.Message);

        // Verify interactions
        _userRepositoryMock.Verify(r => r.GetByUsernameAsync(_mockInputUser.Username), Times.Once);
        _passwordHasherMock.Verify(h => h.VerifyPassword(RawPassword, HashedPassword), Times.Once);
    }

    #endregion
}