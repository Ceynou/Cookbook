using Cookbook.API.Controllers;
using Cookbook.Core;
using Cookbook.SharedData.Contracts.Requests;
using Cookbook.SharedData.Contracts.Responses;
using Cookbook.SharedData.Entities;
using Cookbook.SharedData.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Cookbook.API.UnitTests;

public class AuthenticationControllerTests
{
    private readonly AuthenticationController _controller;
    private readonly Mock<IAccessService> _mockAccessService;
    private readonly Mock<IJwtService> _mockJwtService;
    private readonly Mock<IValidator<SignInUserRequest>> _mockSignInValidator;
    private readonly Mock<IValidator<SignUpUserRequest>> _mockSignUpValidator;

    public AuthenticationControllerTests()
    {
        _mockJwtService = new Mock<IJwtService>();
        _mockAccessService = new Mock<IAccessService>();
        _mockSignUpValidator = new Mock<IValidator<SignUpUserRequest>>();
        _mockSignInValidator = new Mock<IValidator<SignInUserRequest>>();
        _controller = new AuthenticationController(_mockJwtService.Object, _mockAccessService.Object);
    }

    #region SignUp Tests

    [Fact]
    public async Task SignUp_WithValidRequest_ReturnsOkWithToken()
    {
        // Arrange
        var request = new SignUpUserRequest
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123"
        };

        var user = new User
        {
            UserId = 1,
            Username = "testuser",
            Email = "test@example.com",
            IsAdmin = false
        };

        _mockAccessService
            .Setup(s => s.SignUpAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        _mockJwtService
            .Setup(j => j.GenerateJwt("1", It.IsAny<string[]>()))
            .Returns("test-jwt-token");

        // Act
        var result = await _controller.SignUp(_mockSignUpValidator.Object, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<JwtResponse>(okResult.Value);
        Assert.Equal("test-jwt-token", response.Token);
    }

    [Fact]
    public async Task SignUp_WithAdminUser_ReturnsTokenWithAdminAndUserRoles()
    {
        // Arrange
        var request = new SignUpUserRequest
        {
            Username = "adminuser",
            Email = "admin@example.com",
            Password = "password123"
        };

        var adminUser = new User
        {
            UserId = 2,
            Username = "adminuser",
            Email = "admin@example.com",
            IsAdmin = true
        };

        _mockAccessService
            .Setup(s => s.SignUpAsync(It.IsAny<User>()))
            .ReturnsAsync(adminUser);

        _mockJwtService
            .Setup(j => j.GenerateJwt("2", new[] { "admin", "user" }))
            .Returns("admin-jwt-token");

        // Act
        var result = await _controller.SignUp(_mockSignUpValidator.Object, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<JwtResponse>(okResult.Value);
        Assert.Equal("admin-jwt-token", response.Token);

        _mockJwtService.Verify(j => j.GenerateJwt("2", new[] { "admin", "user" }), Times.Once);
    }

    [Fact]
    public async Task SignUp_WhenAccessServiceThrowsDuplicatePropertyException_PropagatesException()
    {
        // Arrange
        var request = new SignUpUserRequest
        {
            Username = "existinguser",
            Email = "existing@example.com",
            Password = "password123"
        };

        _mockAccessService
            .Setup(s => s.SignUpAsync(It.IsAny<User>()))
            .ThrowsAsync(new DuplicatePropertyException("Username", "existinguser"));

        // Act & Assert
        await Assert.ThrowsAsync<DuplicatePropertyException>(() =>
            _controller.SignUp(_mockSignUpValidator.Object, request));
    }

    #endregion

    #region SignIn Tests

    [Fact]
    public async Task SignIn_WithValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var request = new SignInUserRequest
        {
            Username = "testuser",
            Password = "password123"
        };

        var user = new User
        {
            UserId = 1,
            Username = "testuser",
            Email = "test@example.com",
            IsAdmin = false
        };

        _mockAccessService
            .Setup(s => s.SignInAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        _mockJwtService
            .Setup(j => j.GenerateJwt("1", It.IsAny<string[]>()))
            .Returns("signin-jwt-token");

        // Act
        var result = await _controller.SignIn(_mockSignInValidator.Object, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<JwtResponse>(okResult.Value);
        Assert.Equal("signin-jwt-token", response.Token);
    }


    [Fact]
    public async Task SignIn_WithInvalidCredentials_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var request = new SignInUserRequest
        {
            Username = "wronguser",
            Password = "wrongpassword"
        };

        _mockAccessService
            .Setup(s => s.SignInAsync(It.IsAny<User>()))
            .ThrowsAsync(new InvalidCredentialsException());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _controller.SignIn(_mockSignInValidator.Object, request));
    }

    #endregion
}