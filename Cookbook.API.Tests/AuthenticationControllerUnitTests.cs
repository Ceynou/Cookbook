using Cookbook.API.Controllers;
using Cookbook.Core;
using Cookbook.SharedData;
using Cookbook.SharedData.Contracts.Requests;
using Cookbook.SharedData.Contracts.Responses;
using Cookbook.SharedData.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Cookbook.API.Tests;

// ----------------------------------------------------------------------
// UNIT TESTS
// ----------------------------------------------------------------------

public class AuthenticationControllerUnitTests
{
    private const string TestToken = "fake.jwt.token";
    private readonly Mock<IAccessService> _mockAccessService = new();
    private readonly Mock<IJwtService> _mockJwtService = new();
    private readonly Mock<IValidator<SignInUserRequest>> _mockSignInValidator = new();
    private readonly Mock<IValidator<SignUpUserRequest>> _mockSignUpValidator = new();
    private readonly AuthenticationController _sut;

    public AuthenticationControllerUnitTests()
    {
        // Arrange - Setup the controller with mocked dependencies
        _sut = new AuthenticationController(_mockJwtService.Object, _mockAccessService.Object);

        // Standard setup for JWT generation
        _mockJwtService.Setup(s => s.GenerateJwt(
            It.IsAny<string>(),
            It.IsAny<string[]>()
        )).Returns(TestToken);
    }

    // --- Common Helper to Setup Successful Validation ---
    private void SetupSuccessfulValidation<T>(Mock<IValidator<T>> mockValidator) where T : notnull
    {
        mockValidator.Setup(v => v.ValidateAsync(
            It.IsAny<T>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(new ValidationResult());
    }

    // --- Common Helper to Setup Failed Validation ---
    private void SetupFailedValidation<T>(Mock<IValidator<T>> mockValidator) where T : notnull
    {
        mockValidator.Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<T>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException(new List<ValidationFailure>()));
    }

    // ------------------------------------------------------------
    // SIGN UP TESTS
    // ------------------------------------------------------------

    [Fact]
    public async Task SignUp_ValidRequest_ReturnsOkWithJwtResponseAndUserRole()
    {
        // Arrange
        var request = new SignUpUserRequest { Username = "newuser", Email = "a@b.com", Password = "Password123" };
        var createdUser = new User { UserId = 10, Username = request.Username, IsAdmin = false };

        SetupSuccessfulValidation(_mockSignUpValidator);
        _mockAccessService.Setup(s => s.SignUpAsync(It.IsAny<User>()))
            .ReturnsAsync(createdUser);

        // Act
        var result = await _sut.SignUp(_mockSignUpValidator.Object, request);

        // Assert
        // 1. Verify the HTTP response type (200 OK)
        var okResult = Assert.IsType<OkObjectResult>(result);

        // 2. Verify the response body type
        var jwtResponse = Assert.IsType<JwtResponse>(okResult.Value);
        Assert.Equal(TestToken, jwtResponse.Token);

        // 3. Verify internal dependencies were called
        _mockAccessService.Verify(a => a.SignUpAsync(It.IsAny<User>()), Times.Once);

        // 4. Verify JwtService was called with the correct user ID and ROLE
        _mockJwtService.Verify(j => j.GenerateJwt(
            createdUser.UserId.ToString(),
            It.Is<string[]>(roles => roles.Length == 1 && roles.Contains("user"))
        ), Times.Once);
    }

    [Fact]
    public async Task SignUp_AdminUser_ReturnsOkWithAdminAndUserRoles()
    {
        // Arrange
        var request = new SignUpUserRequest { Username = "adminuser", Email = "a@b.com", Password = "Password123" };
        var createdUser = new User { UserId = 20, Username = request.Username, IsAdmin = true }; // IsAdmin = true

        SetupSuccessfulValidation(_mockSignUpValidator);
        _mockAccessService.Setup(s => s.SignUpAsync(It.IsAny<User>()))
            .ReturnsAsync(createdUser);

        // Act
        await _sut.SignUp(_mockSignUpValidator.Object, request);

        // Assert
        // Verify JwtService was called with both "admin" and "user" roles
        _mockJwtService.Verify(j => j.GenerateJwt(
            createdUser.UserId.ToString(),
            It.Is<string[]>(roles => roles.Length == 2 && roles.Contains("user") && roles.Contains("admin"))
        ), Times.Once);
    }

    [Fact]
    public async Task SignUp_ValidationFails_ThrowsValidationException()
    {
        // Arrange
        var request = new SignUpUserRequest { Username = "", Email = "a@b.com", Password = "123" };
        SetupFailedValidation(_mockSignUpValidator);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _sut.SignUp(_mockSignUpValidator.Object, request));

        // Verify core logic was not executed
        _mockAccessService.Verify(a => a.SignUpAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task SignUp_AccessServiceThrows_ExceptionIsPropagated()
    {
        // Arrange
        var request = new SignUpUserRequest { Username = "duplicate", Email = "a@b.com", Password = "Password123" };
        var serviceException = new Exception("Database unique constraint violation.");

        SetupSuccessfulValidation(_mockSignUpValidator);
        _mockAccessService.Setup(s => s.SignUpAsync(It.IsAny<User>()))
            .ThrowsAsync(serviceException);

        // Act & Assert
        var thrownException = await Assert.ThrowsAsync<Exception>(() =>
            _sut.SignUp(_mockSignUpValidator.Object, request));

        Assert.Equal(serviceException.Message, thrownException.Message);
        _mockJwtService.Verify(j => j.GenerateJwt(It.IsAny<string>(), It.IsAny<string[]>()), Times.Never);
    }

    // ------------------------------------------------------------
    // SIGN IN TESTS
    // ------------------------------------------------------------

    [Fact]
    public async Task SignIn_ValidCredentials_ReturnsOkWithJwtResponse()
    {
        // Arrange
        var request = new SignInUserRequest { Username = "existinguser", Password = "Password123" };
        var authenticatedUser = new User { UserId = 30, Username = request.Username, IsAdmin = false };

        SetupSuccessfulValidation(_mockSignInValidator);
        _mockAccessService.Setup(s => s.SignInAsync(It.IsAny<User>()))
            .ReturnsAsync(authenticatedUser);

        // Act
        var result = await _sut.SignIn(_mockSignInValidator.Object, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var jwtResponse = Assert.IsType<JwtResponse>(okResult.Value);
        Assert.Equal(TestToken, jwtResponse.Token);

        _mockAccessService.Verify(a => a.SignInAsync(It.IsAny<User>()), Times.Once);

        // Verify JwtService was called with the correct user ID and ROLE
        _mockJwtService.Verify(j => j.GenerateJwt(
            authenticatedUser.UserId.ToString(),
            It.Is<string[]>(roles => roles.Length == 1 && roles.Contains("user"))
        ), Times.Once);
    }

    [Fact]
    public async Task SignIn_InvalidCredentials_ThrowsException()
    {
        // Arrange
        var request = new SignInUserRequest { Username = "existinguser", Password = "WrongPassword" };

        SetupSuccessfulValidation(_mockSignInValidator);
        // Based on AccessService.cs, an Exception("Invalid credentials") is thrown
        _mockAccessService.Setup(s => s.SignInAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception("Invalid credentials"));

        // Act & Assert
        var thrownException = await Assert.ThrowsAsync<Exception>(() =>
            _sut.SignIn(_mockSignInValidator.Object, request));

        Assert.Equal("Invalid credentials", thrownException.Message);
        _mockJwtService.Verify(j => j.GenerateJwt(It.IsAny<string>(), It.IsAny<string[]>()), Times.Never);
    }

    [Fact]
    public async Task SignIn_UserNotFound_ThrowsResourceNotFoundException()
    {
        // Arrange
        var request = new SignInUserRequest { Username = "nonexistent", Password = "Password123" };

        SetupSuccessfulValidation(_mockSignInValidator);
        // Based on AccessService.cs, a ResourceNotFoundException is thrown
        _mockAccessService.Setup(s => s.SignInAsync(It.IsAny<User>()))
            .ThrowsAsync(new ResourceNotFoundException(typeof(User), "Username", request.Username));

        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() =>
            _sut.SignIn(_mockSignInValidator.Object, request));

        _mockJwtService.Verify(j => j.GenerateJwt(It.IsAny<string>(), It.IsAny<string[]>()), Times.Never);
    }

    [Fact]
    public async Task SignIn_ValidationFails_ThrowsValidationException()
    {
        // Arrange
        var request = new SignInUserRequest { Username = "", Password = "" };
        SetupFailedValidation(_mockSignInValidator);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _sut.SignIn(_mockSignInValidator.Object, request));

        // Verify core logic was not executed
        _mockAccessService.Verify(a => a.SignInAsync(It.IsAny<User>()), Times.Never);
    }
}