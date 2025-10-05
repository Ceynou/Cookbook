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

public class IngredientsControllerUnitTests
{
    private readonly Mock<ICookbookService> _cookbookServiceMock;

    private readonly CreateIngredientRequest _mockCreateRequest = new()
    {
        Name = "Pepper"
    };

    // Mock Entities and Requests
    private readonly Ingredient _mockIngredient = new()
    {
        IngredientId = 1,
        Name = "Salt"
    };

    private readonly UpdateIngredientRequest _mockUpdateRequest = new()
    {
        Name = "Sugar"
    };

    private readonly IngredientsController _sut; // System Under Test


    public IngredientsControllerUnitTests()
    {
        // Use MockBehavior.Strict to ensure all service calls are intentionally handled
        _cookbookServiceMock = new Mock<ICookbookService>(MockBehavior.Strict);
        _sut = new IngredientsController(_cookbookServiceMock.Object);

        // Setup common mocks for void/Task methods
        _cookbookServiceMock.Setup(s => s.DeleteIngredientAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
    }

    #region GetAll Tests

    [Fact]
    public async Task GetAll_WhenIngredientsExist_Should_Return_Ok_With_IngredientsList()
    {
        // ARRANGE
        var ingredientsList = new List<Ingredient> { _mockIngredient, new() { IngredientId = 2, Name = "Water" } };
        _cookbookServiceMock.Setup(s => s.GetAllIngredientsAsync()).ReturnsAsync(ingredientsList);

        // ACT
        var actualResult = await _sut.GetAll();

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsAssignableFrom<IEnumerable<IngredientResponse>>(okResult.Value);
        Assert.Equal(2, response.Count());
        _cookbookServiceMock.Verify(s => s.GetAllIngredientsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenNoIngredientsExist_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        // CookbookService throws RNF if the list is empty
        _cookbookServiceMock.Setup(s => s.GetAllIngredientsAsync())
            .ThrowsAsync(new ResourceNotFoundException(typeof(IEnumerable<Ingredient>)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetAll());
        _cookbookServiceMock.Verify(s => s.GetAllIngredientsAsync(), Times.Once);
    }

    #endregion

    #region GetBy Tests

    [Fact]
    public async Task GetBy_With_GoodId_Should_Return_Ok_With_Ingredient()
    {
        // ARRANGE
        _cookbookServiceMock.Setup(s => s.GetIngredientByAsync(_mockIngredient.IngredientId))
            .ReturnsAsync(_mockIngredient);

        // ACT
        var actualResult = await _sut.GetBy(_mockIngredient.IngredientId);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<IngredientResponse>(okResult.Value);
        Assert.Equal(_mockIngredient.IngredientId, response.IngredientId);
        Assert.Equal(_mockIngredient.Name, response.Name);
    }

    [Fact]
    public async Task GetBy_With_BadId_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        // CookbookService throws RNF if ingredient is not found
        _cookbookServiceMock.Setup(s => s.GetIngredientByAsync(badId))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Ingredient)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetBy(badId));
        _cookbookServiceMock.Verify(s => s.GetIngredientByAsync(badId), Times.Once);
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_With_ValidRequest_Should_Return_CreatedObject()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<CreateIngredientRequest>>();
        var createdIngredientWithId = _mockCreateRequest.ToIngredient();
        createdIngredientWithId.IngredientId = 101;

        // Mock service to return the ingredient with a generated ID
        _cookbookServiceMock
            .Setup(s => s.CreateIngredientAsync(It.IsAny<Ingredient>()))
            .ReturnsAsync(createdIngredientWithId);

        // ACT
        var actualResult = await _sut.Create(validatorMock, _mockCreateRequest);

        // ASSERT
        var createdResult = Assert.IsType<CreatedAtActionResult>(actualResult);
        var response = Assert.IsType<IngredientResponse>(createdResult.Value);

        Assert.Equal(nameof(_sut.GetBy), createdResult.ActionName);
        Assert.Equal(createdIngredientWithId.IngredientId, response.IngredientId);
        _cookbookServiceMock.Verify(s => s.CreateIngredientAsync(It.IsAny<Ingredient>()), Times.Once);
    }

    [Fact]
    public async Task Create_With_InvalidRequest_Should_Throw_ValidationException()
    {
        // ARRANGE
        var validatorMock = new Mock<IValidator<CreateIngredientRequest>>();

        // Create the exception that ValidateAndThrowAsync would generate.
        var validationFailures = new List<ValidationFailure> { new("Name", "Name is required") };
        var expectedException = new ValidationException(validationFailures);

        // Force the ValidateAsync setup to throw the ValidationException directly
        validatorMock
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<CreateIngredientRequest>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Add a redundant service setup to satisfy the Strict Mock (will not be called)
        _cookbookServiceMock
            .Setup(s => s.CreateIngredientAsync(It.IsAny<Ingredient>()))
            .ReturnsAsync(new Ingredient());

        // ACT & ASSERT
        await Assert.ThrowsAsync<ValidationException>(() => _sut.Create(validatorMock.Object, _mockCreateRequest));

        // Verify service was NOT called
        _cookbookServiceMock.Verify(s => s.CreateIngredientAsync(It.IsAny<Ingredient>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenServiceFailsToCreate_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<CreateIngredientRequest>>();

        // Mock service to throw when creation fails (simulating behavior from CookbookService)
        _cookbookServiceMock
            .Setup(s => s.CreateIngredientAsync(It.IsAny<Ingredient>()))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Ingredient), nameof(Ingredient.Name),
                _mockCreateRequest.Name));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.Create(validatorMock, _mockCreateRequest));
        _cookbookServiceMock.Verify(s => s.CreateIngredientAsync(It.IsAny<Ingredient>()), Times.Once);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_With_ValidRequest_Should_Return_Ok_With_UpdatedIngredient()
    {
        // ARRANGE
        const int id = 1;
        var validatorMock = Mock.Of<IValidator<UpdateIngredientRequest>>();
        var updatedIngredient = _mockUpdateRequest.ToIngredient();
        updatedIngredient.IngredientId = id;

        // Mock service to return the successfully modified entity
        _cookbookServiceMock
            .Setup(s => s.ModifyIngredientAsync(id, It.IsAny<Ingredient>()))
            .ReturnsAsync(updatedIngredient);

        // ACT
        var actualResult = await _sut.Update(validatorMock, id, _mockUpdateRequest);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<IngredientResponse>(okResult.Value);
        Assert.Equal("Sugar", response.Name);
        _cookbookServiceMock.Verify(s => s.ModifyIngredientAsync(id, It.IsAny<Ingredient>()), Times.Once);
    }

    [Fact]
    public async Task Update_With_InvalidRequest_Should_Throw_ValidationException()
    {
        // ARRANGE
        const int id = 1;
        var validatorMock = new Mock<IValidator<UpdateIngredientRequest>>();

        // Create the exception that ValidateAndThrowAsync would generate.
        var validationFailures = new List<ValidationFailure> { new("Unit", "Unit cannot be null") };
        var expectedException = new ValidationException(validationFailures);

        // Force the ValidateAsync setup to throw the ValidationException directly
        validatorMock
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<UpdateIngredientRequest>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Add a redundant service setup to satisfy the Strict Mock (will not be called)
        _cookbookServiceMock
            .Setup(s => s.ModifyIngredientAsync(id, It.IsAny<Ingredient>()))
            .ReturnsAsync(new Ingredient());

        // ACT & ASSERT
        await Assert.ThrowsAsync<ValidationException>(() => _sut.Update(validatorMock.Object, id, _mockUpdateRequest));

        // Verify service was NOT called
        _cookbookServiceMock.Verify(s => s.ModifyIngredientAsync(It.IsAny<int>(), It.IsAny<Ingredient>()), Times.Never);
    }

    [Fact]
    public async Task Update_WhenIngredientDoesNotExist_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        var validatorMock = Mock.Of<IValidator<UpdateIngredientRequest>>();

        // Mock service to throw when resource is not found
        _cookbookServiceMock
            .Setup(s => s.ModifyIngredientAsync(badId, It.IsAny<Ingredient>()))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Ingredient)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() =>
            _sut.Update(validatorMock, badId, _mockUpdateRequest));
        _cookbookServiceMock.Verify(s => s.ModifyIngredientAsync(badId, It.IsAny<Ingredient>()), Times.Once);
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
        _cookbookServiceMock.Verify(s => s.DeleteIngredientAsync(id), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenIngredientDoesNotExist_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        // Mock service to throw when the resource is not found
        _cookbookServiceMock.Setup(s => s.DeleteIngredientAsync(badId))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Ingredient)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.Delete(badId));
        _cookbookServiceMock.Verify(s => s.DeleteIngredientAsync(badId), Times.Once);
    }

    #endregion
}