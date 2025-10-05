using Cookbook.API.Controllers;
using Cookbook.Core;
using Cookbook.SharedData.Contracts.Requests;
using Cookbook.SharedData.Contracts.Responses;
using Cookbook.SharedData.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

// NOTE: IAccessService and IJwtService are assumed based on their usage in AuthenticationController.cs
// They are mocked here to test the controller's logic.

namespace Cookbook.API.Tests;

public class AuthenticationControllerUnitTests
{
    // Mock Data
    private const string MockToken = "mock.jwt.token";
    private readonly Mock<IAccessService> _accessServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;

    private readonly User _mockAdminUser = new()
    {
        UserId = 10,
        Username = "adminuser",
        IsAdmin = true,
        BirthDate = DateOnly.MinValue,
        Email = "admin1@admin.fr"
    };

    private readonly User _mockRegularUser = new()
    {
        UserId = 11,
        Username = "regularuser",
        IsAdmin = false,
        BirthDate = DateOnly.MinValue,
        Email = "user20@user.fr"
    };

    private readonly SignInUserRequest _mockSignInRequest = new()
    {
        Username = "existinguser",
        Password = "P@ssword1"
    };

    private readonly SignUpUserRequest _mockSignUpRequest = new()
    {
        Username = "newuser",
        Password = "P@ssword1",
        Email = "something@er.fr",
        BirthDate = DateOnly.MinValue
    };

    private readonly AuthenticationController _sut; // System Under Test

    public AuthenticationControllerUnitTests()
    {
        // Use MockBehavior.Strict for all service mocks
        _jwtServiceMock = new Mock<IJwtService>(MockBehavior.Strict);
        _accessServiceMock = new Mock<IAccessService>(MockBehavior.Strict);
        _sut = new AuthenticationController(_jwtServiceMock.Object, _accessServiceMock.Object);

        // Setup the JWT service to always return the mock token
        _jwtServiceMock
            .Setup(j => j.GenerateJwt(It.IsAny<string>(), It.IsAny<string[]>()))
            .Returns(MockToken);
    }

    #region SignUp Tests

    [Fact]
    public async Task SignUp_WhenValidRequestCreatesAdminUser_Should_Return_Ok_With_AdminToken()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<SignUpUserRequest>>();

        // Setup Access Service to return an ADMIN user
        _accessServiceMock
            .Setup(a => a.SignUpAsync(It.IsAny<User>()))
            .ReturnsAsync(_mockAdminUser);

        // ACT
        var actualResult = await _sut.SignUp(validatorMock, _mockSignUpRequest);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<SignUpUserResponse>(okResult.Value);

        Assert.Equal(MockToken, response.Token);
        Assert.Equal(_mockAdminUser.UserId, response.UserId);

        // Verify JWT service was called with 'admin' and 'user' roles
        _jwtServiceMock.Verify(
            j => j.GenerateJwt(
                _mockAdminUser.UserId.ToString(),
                It.Is<string[]>(roles => roles.Contains("admin") && roles.Contains("user"))
            ),
            Times.Once);
        _accessServiceMock.Verify(a => a.SignUpAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task SignUp_WhenValidRequestCreatesRegularUser_Should_Return_Ok_With_UserToken()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<SignUpUserRequest>>();

        // Setup Access Service to return a REGULAR user
        _accessServiceMock
            .Setup(a => a.SignUpAsync(It.IsAny<User>()))
            .ReturnsAsync(_mockRegularUser);

        // ACT
        var actualResult = await _sut.SignUp(validatorMock, _mockSignUpRequest);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<SignUpUserResponse>(okResult.Value);

        Assert.Equal(MockToken, response.Token);
        Assert.Equal(_mockRegularUser.UserId, response.UserId);

        // Verify JWT service was called ONLY with 'user' role
        _jwtServiceMock.Verify(
            j => j.GenerateJwt(
                _mockRegularUser.UserId.ToString(),
                It.Is<string[]>(roles => roles.Length == 1 && roles.Contains("user"))
            ),
            Times.Once);
        _accessServiceMock.Verify(a => a.SignUpAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task SignUp_With_InvalidRequest_Should_Throw_ValidationException()
    {
        // ARRANGE
        var validatorMock = new Mock<IValidator<SignUpUserRequest>>();

        // Create the exception that ValidateAndThrowAsync would generate.
        var validationFailures = new List<ValidationFailure> { new("Password", "Password is too weak") };
        var expectedException = new ValidationException(validationFailures);

        // Force the ValidateAsync setup to throw the ValidationException directly
        validatorMock
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<SignUpUserRequest>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Add redundant service setups to satisfy the Strict Mock (will not be called)
        _accessServiceMock.Setup(a => a.SignUpAsync(It.IsAny<User>())).ReturnsAsync(_mockRegularUser);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ValidationException>(() => _sut.SignUp(validatorMock.Object, _mockSignUpRequest));

        // Verify services were NOT called
        _accessServiceMock.Verify(a => a.SignUpAsync(It.IsAny<User>()), Times.Never);
        _jwtServiceMock.Verify(j => j.GenerateJwt(It.IsAny<string>(), It.IsAny<string[]>()), Times.Never);
    }

    [Fact]
    public async Task SignUp_WhenAccessServiceThrows_Should_PropagateException()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<SignUpUserRequest>>();

        // Mock service to throw when signing up (e.g., username already exists)
        var serviceException = new Exception("Username already exists.");
        _accessServiceMock
            .Setup(a => a.SignUpAsync(It.IsAny<User>()))
            .ThrowsAsync(serviceException);

        // ACT & ASSERT
        var actualException = await Assert.ThrowsAsync<Exception>(() => _sut.SignUp(validatorMock, _mockSignUpRequest));
        Assert.Equal(serviceException.Message, actualException.Message);

        // Verify services were called appropriately
        _accessServiceMock.Verify(a => a.SignUpAsync(It.IsAny<User>()), Times.Once);
        _jwtServiceMock.Verify(j => j.GenerateJwt(It.IsAny<string>(), It.IsAny<string[]>()), Times.Never);
    }

    #endregion

    // -------------------------------------------------------------------------

    #region SignIn Tests

    [Fact]
    public async Task SignIn_WhenValidRequestAuthenticatesAdminUser_Should_Return_Ok_With_AdminToken()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<SignInUserRequest>>();

        // Setup Access Service to return an ADMIN user
        _accessServiceMock
            .Setup(a => a.SignInAsync(It.IsAny<User>()))
            .ReturnsAsync(_mockAdminUser);

        // ACT
        var actualResult = await _sut.SignIn(validatorMock, _mockSignInRequest);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<SignInUserResponse>(okResult.Value);

        Assert.Equal(MockToken, response.Token);
        Assert.Equal(_mockAdminUser.UserId, response.UserId);

        // Verify JWT service was called with 'admin' and 'user' roles
        _jwtServiceMock.Verify(
            j => j.GenerateJwt(
                _mockAdminUser.UserId.ToString(),
                It.Is<string[]>(roles => roles.Contains("admin") && roles.Contains("user"))
            ),
            Times.Once);
        _accessServiceMock.Verify(a => a.SignInAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task SignIn_WhenValidRequestAuthenticatesRegularUser_Should_Return_Ok_With_UserToken()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<SignInUserRequest>>();

        // Setup Access Service to return a REGULAR user
        _accessServiceMock
            .Setup(a => a.SignInAsync(It.IsAny<User>()))
            .ReturnsAsync(_mockRegularUser);

        // ACT
        var actualResult = await _sut.SignIn(validatorMock, _mockSignInRequest);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<SignInUserResponse>(okResult.Value);

        Assert.Equal(MockToken, response.Token);
        Assert.Equal(_mockRegularUser.UserId, response.UserId);

        // Verify JWT service was called ONLY with 'user' role
        _jwtServiceMock.Verify(
            j => j.GenerateJwt(
                _mockRegularUser.UserId.ToString(),
                It.Is<string[]>(roles => roles.Length == 1 && roles.Contains("user"))
            ),
            Times.Once);
        _accessServiceMock.Verify(a => a.SignInAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task SignIn_With_InvalidRequest_Should_Throw_ValidationException()
    {
        // ARRANGE
        var validatorMock = new Mock<IValidator<SignInUserRequest>>();

        // Create the exception that ValidateAndThrowAsync would generate.
        var validationFailures = new List<ValidationFailure> { new("Username", "Username is too short") };
        var expectedException = new ValidationException(validationFailures);

        // Force the ValidateAsync setup to throw the ValidationException directly
        validatorMock
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<SignInUserRequest>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Add redundant service setups to satisfy the Strict Mock (will not be called)
        _accessServiceMock.Setup(a => a.SignInAsync(It.IsAny<User>())).ReturnsAsync(_mockRegularUser);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ValidationException>(() => _sut.SignIn(validatorMock.Object, _mockSignInRequest));

        // Verify services were NOT called
        _accessServiceMock.Verify(a => a.SignInAsync(It.IsAny<User>()), Times.Never);
        _jwtServiceMock.Verify(j => j.GenerateJwt(It.IsAny<string>(), It.IsAny<string[]>()), Times.Never);
    }

    [Fact]
    public async Task SignIn_WhenAccessServiceThrowsAuthenticationFailure_Should_PropagateException()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<SignInUserRequest>>();

        // Mock service to throw when signing in (e.g., bad password/username, leading to Unauthorized in API layer)
        var serviceException = new Exception("Invalid credentials.");
        _accessServiceMock
            .Setup(a => a.SignInAsync(It.IsAny<User>()))
            .ThrowsAsync(serviceException);

        // ACT & ASSERT
        var actualException = await Assert.ThrowsAsync<Exception>(() => _sut.SignIn(validatorMock, _mockSignInRequest));
        Assert.Equal(serviceException.Message, actualException.Message);

        // Verify services were called appropriately
        _accessServiceMock.Verify(a => a.SignInAsync(It.IsAny<User>()), Times.Once);
        _jwtServiceMock.Verify(j => j.GenerateJwt(It.IsAny<string>(), It.IsAny<string[]>()), Times.Never);
    }

    #endregion
}