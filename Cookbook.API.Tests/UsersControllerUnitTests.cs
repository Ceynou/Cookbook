using Cookbook.API.Controllers;
using Cookbook.Core;
using Cookbook.SharedData;
using Cookbook.SharedData.Contracts.Requests;
using Cookbook.SharedData.Contracts.Responses;
using Cookbook.SharedData.Entities;
using Cookbook.SharedData.Mappers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Cookbook.API.Tests;

public class UsersControllerUnitTests
{
    private readonly Mock<ICookbookService> _cookbookServiceMock;

    private readonly CreateUserRequest _mockCreateRequest = new()
    {
        Username = "newuser",
        Email = "something@mail.fr",
        IsAdmin = false,
        Password = "SecurePassword123"
    };

    private readonly UpdateUserRequest _mockUpdateRequest = new()
    {
        Username = "updateduser",
        Email = "something@mail.fr",
        IsAdmin = false,
        Password = "SecurePassword123"
    };

    // Mock Entities and Requests
    private readonly User _mockUser = new()
    {
        UserId = 1,
        Username = "testuser",
        Email = "something@mail.fr",
        ImagePath = "None",
        IsAdmin = false
    };

    private readonly UsersController _sut; // System Under Test


    public UsersControllerUnitTests()
    {
        // Use MockBehavior.Strict to ensure all service calls are intentionally handled
        _cookbookServiceMock = new Mock<ICookbookService>(MockBehavior.Strict);
        _sut = new UsersController(_cookbookServiceMock.Object);

        // Setup common mocks for void/Task methods
        _cookbookServiceMock.Setup(s => s.DeleteUserAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
    }


    #region GetAll Tests

    [Fact]
    public async Task GetAll_WhenUsersExist_Should_Return_Ok_With_UsersList()
    {
        // ARRANGE
        var usersList = new List<User> { _mockUser, new() { UserId = 2, Username = "user2" } };
        _cookbookServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(usersList);

        // ACT
        var actualResult = await _sut.GetAll();

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsAssignableFrom<IEnumerable<UserResponse>>(okResult.Value);
        Assert.Equal(2, response.Count());
        _cookbookServiceMock.Verify(s => s.GetAllUsersAsync(), Times.Once);
    }

    // NOTE: The CookbookService.GetAllUsersAsync doesn't throw a ResourceNotFoundException 
    // when the list is empty, it returns an empty list (unlike GetAllRecipesAsync).
    // Test for empty list returning an empty OK 200 array.
    [Fact]
    public async Task GetAll_WhenNoUsersExist_Should_Return_Ok_With_EmptyList()
    {
        // ARRANGE
        var usersList = new List<User>();
        _cookbookServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(usersList);

        // ACT
        var actualResult = await _sut.GetAll();

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsAssignableFrom<IEnumerable<UserResponse>>(okResult.Value);
        Assert.Empty(response);
    }

    #endregion

    #region GetBy Tests

    [Fact]
    public async Task GetBy_With_GoodId_Should_Return_Ok_With_User()
    {
        // ARRANGE
        _cookbookServiceMock.Setup(s => s.GetUserByAsync(_mockUser.UserId)).ReturnsAsync(_mockUser);

        // ACT
        var actualResult = await _sut.GetBy(_mockUser.UserId);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<UserResponse>(okResult.Value);
        Assert.Equal(_mockUser.UserId, response.UserId);
        Assert.Equal(_mockUser.Username, response.Username);
    }

    [Fact]
    public async Task GetBy_With_BadId_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        _cookbookServiceMock.Setup(s => s.GetUserByAsync(badId))
            .ThrowsAsync(new ResourceNotFoundException(typeof(User)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetBy(badId));
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_With_ValidRequest_Should_Return_CreatedObject()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<CreateUserRequest>>();
        var createdUserWithId = _mockCreateRequest.ToUser();
        createdUserWithId.UserId = 101;

        // Mock service to return the user with a generated ID
        _cookbookServiceMock
            .Setup(s => s.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(createdUserWithId);

        // ACT
        var actualResult = await _sut.Create(validatorMock, _mockCreateRequest);

        // ASSERT
        var createdResult = Assert.IsType<CreatedAtActionResult>(actualResult);
        var response = Assert.IsType<UserResponse>(createdResult.Value);

        Assert.Equal(nameof(_sut.GetBy), createdResult.ActionName);
        Assert.Equal(createdUserWithId.UserId, response.UserId);
        _cookbookServiceMock.Verify(s => s.CreateUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Create_With_InvalidRequest_Should_Throw_ValidationException()
    {
        // ARRANGE
        var validatorMock = new Mock<IValidator<CreateUserRequest>>();

        // Create the exception that ValidateAndThrowAsync would generate.
        var validationFailures = new List<ValidationFailure> { new("Username", "Username is required") };
        var expectedException = new ValidationException(validationFailures);

        // Force the ValidateAsync setup to throw the ValidationException directly
        validatorMock
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<CreateUserRequest>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Add a redundant service setup to satisfy the Strict Mock (will not be called)
        _cookbookServiceMock
            .Setup(s => s.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(new User());

        // ACT & ASSERT
        await Assert.ThrowsAsync<ValidationException>(() => _sut.Create(validatorMock.Object, _mockCreateRequest));

        // Verify service was NOT called
        _cookbookServiceMock.Verify(s => s.CreateUserAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenServiceFailsToCreate_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<CreateUserRequest>>();

        // Mock service to throw when creation fails (simulating behavior from CookbookService)
        _cookbookServiceMock
            .Setup(s => s.CreateUserAsync(It.IsAny<User>()))
            .ThrowsAsync(new ResourceNotFoundException(typeof(User), "Username", _mockCreateRequest.Username));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.Create(validatorMock, _mockCreateRequest));
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_With_ValidRequest_Should_Return_Ok_With_UpdatedUser()
    {
        // ARRANGE
        const int id = 1;
        var validatorMock = Mock.Of<IValidator<UpdateUserRequest>>();
        var updatedUser = _mockUpdateRequest.ToUser();
        updatedUser.UserId = id;

        // Mock service to return the successfully modified entity
        _cookbookServiceMock
            .Setup(s => s.ModifyUserAsync(id, It.IsAny<User>()))
            .ReturnsAsync(updatedUser);

        // ACT
        var actualResult = await _sut.Update(validatorMock, id, _mockUpdateRequest);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<UserResponse>(okResult.Value);
        Assert.Equal("updateduser", response.Username);
        _cookbookServiceMock.Verify(s => s.ModifyUserAsync(id, It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Update_With_InvalidRequest_Should_Throw_ValidationException()
    {
        // ARRANGE
        const int id = 1;
        var validatorMock = new Mock<IValidator<UpdateUserRequest>>();

        // Create the exception that ValidateAndThrowAsync would generate.
        var validationFailures = new List<ValidationFailure> { new("Username", "Username is too short") };
        var expectedException = new ValidationException(validationFailures);

        // Force the ValidateAsync setup to throw the ValidationException directly
        validatorMock
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<UpdateUserRequest>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Add a redundant service setup to satisfy the Strict Mock (will not be called)
        _cookbookServiceMock
            .Setup(s => s.ModifyUserAsync(id, It.IsAny<User>()))
            .ReturnsAsync(new User());

        // ACT & ASSERT
        await Assert.ThrowsAsync<ValidationException>(() => _sut.Update(validatorMock.Object, id, _mockUpdateRequest));

        // Verify service was NOT called
        _cookbookServiceMock.Verify(s => s.ModifyUserAsync(It.IsAny<int>(), It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Update_WhenUserDoesNotExist_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        var validatorMock = Mock.Of<IValidator<UpdateUserRequest>>();

        // Mock service to throw when resource is not found
        _cookbookServiceMock
            .Setup(s => s.ModifyUserAsync(badId, It.IsAny<User>()))
            .ThrowsAsync(new ResourceNotFoundException(typeof(User)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() =>
            _sut.Update(validatorMock, badId, _mockUpdateRequest));
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_With_GoodId_Should_Return_NoContent()
    {
        // ARRANGE
        const int id = 1;
        // Mock is already set up in the constructor to return Task.CompletedTask

        // ACT
        var actualResult = await _sut.Delete(id);

        // ASSERT
        Assert.IsType<NoContentResult>(actualResult);
        _cookbookServiceMock.Verify(s => s.DeleteUserAsync(id), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenUserDoesNotExist_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        // Mock service to throw when the resource is not found (simulating behavior from CookbookService)
        _cookbookServiceMock.Setup(s => s.DeleteUserAsync(badId))
            .ThrowsAsync(new ResourceNotFoundException(typeof(User)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.Delete(badId));
        _cookbookServiceMock.Verify(s => s.DeleteUserAsync(badId), Times.Once);
    }

    #endregion
}