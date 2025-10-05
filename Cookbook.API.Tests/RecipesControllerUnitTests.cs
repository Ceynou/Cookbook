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

public class RecipesControllerUnitTests
{
    private readonly Mock<ICookbookService> _cookbookServiceMock;

    private readonly CreateRecipeRequest _mockCreateRequest = new()
    {
        Title = "Something",
        ImagePath = "Nothing",
        Difficulty = 3,
        Categories = [new CreateRecipeCategoryRequest { CategoryId = 1 }],
        Ingredients = [new CreateRecipeIngredientRequest { IngredientId = 1, Quantity = 1 }],
        Steps =
        [
            new CreateStepRequest
                { StepNumber = 1, Duration = TimeSpan.FromHours(1), Instruction = "Instruction", IsCooking = false }
        ]
    };

    private readonly Recipe _mockRecipe = new()
    {
        RecipeId = 1,
        Title = "Test Recipe",
        CreatorId = 5,
        Difficulty = 3
    };

    private readonly UpdateRecipeRequest _mockUpdateRequest = new()
    {
        Title = "Updated Title",
        ImagePath = "New Path",
        Difficulty = 2,
        Ingredients = [],
        Categories = [],
        Steps = []
    };

    private readonly RecipesController _sut;

    public RecipesControllerUnitTests()
    {
        // Use MockBehavior.Strict to ensure any un-mocked call throws an exception
        _cookbookServiceMock = new Mock<ICookbookService>(MockBehavior.Strict);
        _sut = new RecipesController(_cookbookServiceMock.Object);

        // Setup common mocks to avoid strict failures where the return type is Task or void
        _cookbookServiceMock.Setup(s => s.DeleteRecipeAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
    }

    #region GetAll Tests

    [Fact]
    public async Task GetAll_WhenRecipesExist_Should_Return_Ok_With_RecipesList()
    {
        // ARRANGE
        var recipesList = new List<Recipe> { _mockRecipe, new() { RecipeId = 2 } };
        _cookbookServiceMock.Setup(s => s.GetAllRecipesAsync()).ReturnsAsync(recipesList);

        // ACT
        var actualResult = await _sut.GetAll();

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsAssignableFrom<IEnumerable<RecipeResponse>>(okResult.Value);
        Assert.Equal(2, response.Count());
        _cookbookServiceMock.Verify(s => s.GetAllRecipesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenNoRecipesExist_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        _cookbookServiceMock.Setup(s => s.GetAllRecipesAsync())
            .ThrowsAsync(new ResourceNotFoundException(typeof(IEnumerable<Recipe>)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetAll());
    }

    #endregion

    #region GetBy (Single) Tests

    [Fact]
    public async Task GetBy_With_GoodId_Should_Return_Ok_With_Recipe()
    {
        // ARRANGE
        _cookbookServiceMock.Setup(s => s.GetRecipeByAsync(_mockRecipe.RecipeId)).ReturnsAsync(_mockRecipe);

        // ACT
        var actualResult = await _sut.GetBy(_mockRecipe.RecipeId);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<RecipeResponse>(okResult.Value);
        Assert.Equal(_mockRecipe.RecipeId, response.RecipeId);
    }

    [Fact]
    public async Task GetBy_With_BadId_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        _cookbookServiceMock.Setup(s => s.GetRecipeByAsync(badId))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Recipe)));

        // ACT & ASSERT
        var exception = await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetBy(badId));
        Assert.Contains("Recipe not found", exception.Message);
    }

    #endregion

    #region GetFullBy (Detail) Tests

    [Fact]
    public async Task GetFullBy_With_GoodId_Should_Return_Ok_With_RecipeDetail()
    {
        // ARRANGE
        _cookbookServiceMock.Setup(s => s.GetRecipeByAsync(_mockRecipe.RecipeId)).ReturnsAsync(_mockRecipe);

        // ACT
        var actualResult = await _sut.GetFullBy(_mockRecipe.RecipeId);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<RecipeDetailResponse>(okResult.Value);
        Assert.Equal(_mockRecipe.RecipeId, response.RecipeId);
    }

    [Fact]
    public async Task GetFullBy_With_BadId_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        _cookbookServiceMock.Setup(s => s.GetRecipeByAsync(badId))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Recipe)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetFullBy(badId));
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_With_ValidRequest_Should_Return_CreatedObject()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<CreateRecipeRequest>>(); // Success validation is implicit (no throw)
        var recipeWithId = _mockCreateRequest.ToRecipe();
        recipeWithId.RecipeId = 101;

        // Mock service to return the recipe with a generated ID
        _cookbookServiceMock
            .Setup(s => s.CreateRecipeAsync(It.IsAny<Recipe>()))
            .ReturnsAsync(recipeWithId);

        // ACT
        var actualResult = await _sut.Create(validatorMock, _mockCreateRequest);

        // ASSERT
        var createdResult = Assert.IsType<CreatedAtActionResult>(actualResult);
        var response = Assert.IsType<RecipeResponse>(createdResult.Value);

        Assert.Equal(nameof(_sut.GetBy), createdResult.ActionName);
        Assert.Equal(recipeWithId.RecipeId, response.RecipeId);
        _cookbookServiceMock.Verify(s => s.CreateRecipeAsync(It.IsAny<Recipe>()), Times.Once);
    }

    [Fact]
    public async Task Create_With_InvalidRequest_Should_Throw_ValidationException()
    {
        // ARRANGE
        const int id = 1;
        var validatorMock = new Mock<IValidator<CreateRecipeRequest>>();
        // Setup the mock validator to throw a ValidationException
        // 1. Force the ValidateAsync setup to throw the ValidationException directly.
        // This correctly simulates the behavior of the ValidateAndThrowAsync extension method.
        validatorMock
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<CreateRecipeRequest>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException(new List<ValidationFailure>()));

        // 2. Add a redundant service setup to satisfy the Strict Mock behavior,
        // although this call will never be reached due to the forced exception above.
        _cookbookServiceMock
            .Setup(s => s.ModifyRecipeAsync(id, It.IsAny<Recipe>()))
            .ReturnsAsync(new Recipe());

        // ACT & ASSERT
        await Assert.ThrowsAsync<ValidationException>(() => _sut.Create(validatorMock.Object, _mockCreateRequest));

        // Verify service was NOT called
        _cookbookServiceMock.Verify(s => s.CreateRecipeAsync(It.IsAny<Recipe>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenServiceFailsToCreate_Should_Throw_Exception()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<CreateRecipeRequest>>();

        // Mock service to throw when creating (e.g., database error, or internal logic failure)
        _cookbookServiceMock
            .Setup(s => s.CreateRecipeAsync(It.IsAny<Recipe>()))
            .ThrowsAsync(new Exception("Database connection failed."));

        // ACT & ASSERT
        await Assert.ThrowsAsync<Exception>(() => _sut.Create(validatorMock, _mockCreateRequest));
    }

    #endregion

    #region Modify Tests

    [Fact]
    public async Task Modify_With_ValidRequest_Should_Return_Ok_With_UpdatedRecipe()
    {
        // ARRANGE
        const int id = 1;
        var validatorMock = Mock.Of<IValidator<UpdateRecipeRequest>>();
        var updatedRecipe = _mockUpdateRequest.ToRecipe();
        updatedRecipe.RecipeId = id;

        // Mock service to return the successfully modified entity
        _cookbookServiceMock
            .Setup(s => s.ModifyRecipeAsync(id, It.IsAny<Recipe>()))
            .ReturnsAsync(updatedRecipe);

        // ACT
        var actualResult = await _sut.Modify(validatorMock, id, _mockUpdateRequest);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<RecipeResponse>(okResult.Value);
        Assert.Equal("Updated Title", response.Title);
        _cookbookServiceMock.Verify(s => s.ModifyRecipeAsync(id, It.IsAny<Recipe>()), Times.Once);
    }

    [Fact]
    public async Task Modify_With_InvalidRequest_Should_Throw_ValidationException()
    {
        // ARRANGE
        const int id = 1;
        var validatorMock = new Mock<IValidator<UpdateRecipeRequest>>();

        // Create the exception that ValidateAndThrowAsync would generate.
        var validationFailures = new List<ValidationFailure> { new("Title", "Title cannot be empty") };
        var expectedException = new ValidationException(validationFailures);

        // 1. Force the ValidateAsync setup to throw the ValidationException directly.
        // This correctly simulates the behavior of the ValidateAndThrowAsync extension method.
        validatorMock
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<UpdateRecipeRequest>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // 2. Add a redundant service setup to satisfy the Strict Mock behavior,
        // although this call will never be reached due to the forced exception above.
        _cookbookServiceMock
            .Setup(s => s.ModifyRecipeAsync(id, It.IsAny<Recipe>()))
            .ReturnsAsync(new Recipe());

        // ACT & ASSERT
        // Now Assert.ThrowsAsync will successfully catch the ValidationException
        await Assert.ThrowsAsync<ValidationException>(() => _sut.Modify(validatorMock.Object, id, _mockUpdateRequest));

        // Verify service was NOT called
        _cookbookServiceMock.Verify(s => s.ModifyRecipeAsync(It.IsAny<int>(), It.IsAny<Recipe>()), Times.Never);
    }

    [Fact]
    public async Task Modify_WhenRecipeDoesNotExist_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        var validatorMock = Mock.Of<IValidator<UpdateRecipeRequest>>();

        // Mock service to throw when resource is not found
        _cookbookServiceMock
            .Setup(s => s.ModifyRecipeAsync(badId, It.IsAny<Recipe>()))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Recipe)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() =>
            _sut.Modify(validatorMock, badId, _mockUpdateRequest));
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
        _cookbookServiceMock.Verify(s => s.DeleteRecipeAsync(id), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenRecipeDoesNotExist_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        // Mock service to throw when the resource is not found
        _cookbookServiceMock.Setup(s => s.DeleteRecipeAsync(badId))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Recipe)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.Delete(badId));
        _cookbookServiceMock.Verify(s => s.DeleteRecipeAsync(badId), Times.Once);
    }

    #endregion
}