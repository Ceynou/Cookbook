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

public class CategoriesControllerUnitTests
{
    private readonly Mock<ICookbookService> _cookbookServiceMock;

    // Mock Entities and Requests
    private readonly Category _mockCategory = new()
    {
        CategoryId = 1,
        Name = "Appetizers"
    };

    private readonly CreateCategoryRequest _mockCreateRequest = new()
    {
        Name = "Desserts"
    };

    private readonly UpdateCategoryRequest _mockUpdateRequest = new()
    {
        Name = "SoupsAndStews"
    };

    private readonly CategoriesController _sut; // System Under Test


    public CategoriesControllerUnitTests()
    {
        // Use MockBehavior.Strict to ensure all service calls are intentionally handled
        _cookbookServiceMock = new Mock<ICookbookService>(MockBehavior.Strict);
        _sut = new CategoriesController(_cookbookServiceMock.Object);

        // Setup common mocks for void/Task methods
        _cookbookServiceMock.Setup(s => s.DeleteCategoryAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
    }

    #region GetAll Tests

    [Fact]
    public async Task GetAll_WhenCategoriesExist_Should_Return_Ok_With_CategoriesList()
    {
        // ARRANGE
        var categoriesList = new List<Category> { _mockCategory, new() { CategoryId = 2, Name = "MainCourses" } };
        _cookbookServiceMock.Setup(s => s.GetAllCategoriesAsync()).ReturnsAsync(categoriesList);

        // ACT
        var actualResult = await _sut.GetAll();

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsAssignableFrom<IEnumerable<CategoryResponse>>(okResult.Value);
        Assert.Equal(2, response.Count());
        _cookbookServiceMock.Verify(s => s.GetAllCategoriesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenNoCategoriesExist_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        // The service throws RNF if the list is empty
        _cookbookServiceMock.Setup(s => s.GetAllCategoriesAsync())
            .ThrowsAsync(new ResourceNotFoundException(typeof(IEnumerable<Category>)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetAll());
        _cookbookServiceMock.Verify(s => s.GetAllCategoriesAsync(), Times.Once);
    }

    #endregion

    #region GetBy Tests

    [Fact]
    public async Task GetBy_With_GoodId_Should_Return_Ok_With_Category()
    {
        // ARRANGE
        _cookbookServiceMock.Setup(s => s.GetCategoryByAsync(_mockCategory.CategoryId)).ReturnsAsync(_mockCategory);

        // ACT
        var actualResult = await _sut.GetBy(_mockCategory.CategoryId);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<CategoryResponse>(okResult.Value);
        Assert.Equal(_mockCategory.CategoryId, response.CategoryId);
        Assert.Equal(_mockCategory.Name, response.Name);
    }

    [Fact]
    public async Task GetBy_With_BadId_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        // The service throws RNF if category is not found
        _cookbookServiceMock.Setup(s => s.GetCategoryByAsync(badId))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Category)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetBy(badId));
        _cookbookServiceMock.Verify(s => s.GetCategoryByAsync(badId), Times.Once);
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_With_ValidRequest_Should_Return_CreatedObject()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<CreateCategoryRequest>>();
        var createdCategoryWithId = _mockCreateRequest.ToCategory();
        createdCategoryWithId.CategoryId = 101;

        // Mock service to return the category with a generated ID
        _cookbookServiceMock
            .Setup(s => s.CreateCategoryAsync(It.IsAny<Category>()))
            .ReturnsAsync(createdCategoryWithId);

        // ACT
        var actualResult = await _sut.Create(validatorMock, _mockCreateRequest);

        // ASSERT
        var createdResult = Assert.IsType<CreatedAtActionResult>(actualResult);
        var response = Assert.IsType<CategoryResponse>(createdResult.Value);

        Assert.Equal(nameof(_sut.GetBy), createdResult.ActionName);
        Assert.Equal(createdCategoryWithId.CategoryId, response.CategoryId);
        _cookbookServiceMock.Verify(s => s.CreateCategoryAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task Create_With_InvalidRequest_Should_Throw_ValidationException()
    {
        // ARRANGE
        var validatorMock = new Mock<IValidator<CreateCategoryRequest>>();

        // Create the exception that ValidateAndThrowAsync would generate.
        var validationFailures = new List<ValidationFailure> { new("Name", "Name is required") };
        var expectedException = new ValidationException(validationFailures);

        // Force the ValidateAsync setup to throw the ValidationException directly
        validatorMock
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<CreateCategoryRequest>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Add a redundant service setup to satisfy the Strict Mock (will not be called)
        _cookbookServiceMock
            .Setup(s => s.CreateCategoryAsync(It.IsAny<Category>()))
            .ReturnsAsync(new Category());

        // ACT & ASSERT
        await Assert.ThrowsAsync<ValidationException>(() => _sut.Create(validatorMock.Object, _mockCreateRequest));

        // Verify service was NOT called
        _cookbookServiceMock.Verify(s => s.CreateCategoryAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenServiceFailsToCreate_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        var validatorMock = Mock.Of<IValidator<CreateCategoryRequest>>();

        // Mock service to throw when creation fails (e.g., duplicate name)
        _cookbookServiceMock
            .Setup(s => s.CreateCategoryAsync(It.IsAny<Category>()))
            .ThrowsAsync(
                new ResourceNotFoundException(typeof(Category), nameof(Category.Name), _mockCreateRequest.Name));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.Create(validatorMock, _mockCreateRequest));
        _cookbookServiceMock.Verify(s => s.CreateCategoryAsync(It.IsAny<Category>()), Times.Once);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_With_ValidRequest_Should_Return_Ok_With_UpdatedCategory()
    {
        // ARRANGE
        const int id = 1;
        var validatorMock = Mock.Of<IValidator<UpdateCategoryRequest>>();
        var updatedCategory = _mockUpdateRequest.ToCategory();
        updatedCategory.CategoryId = id;

        // Mock service to return the successfully modified entity
        _cookbookServiceMock
            .Setup(s => s.ModifyCategoryAsync(id, It.IsAny<Category>()))
            .ReturnsAsync(updatedCategory);

        // ACT
        var actualResult = await _sut.Update(validatorMock, id, _mockUpdateRequest);

        // ASSERT
        var okResult = Assert.IsType<OkObjectResult>(actualResult);
        var response = Assert.IsType<CategoryResponse>(okResult.Value);
        Assert.Equal("SoupsAndStews", response.Name);
        _cookbookServiceMock.Verify(s => s.ModifyCategoryAsync(id, It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task Update_With_InvalidRequest_Should_Throw_ValidationException()
    {
        // ARRANGE
        const int id = 1;
        var validatorMock = new Mock<IValidator<UpdateCategoryRequest>>();

        // Create the exception that ValidateAndThrowAsync would generate.
        var validationFailures = new List<ValidationFailure> { new("Name", "Name is too short") };
        var expectedException = new ValidationException(validationFailures);

        // Force the ValidateAsync setup to throw the ValidationException directly
        validatorMock
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<UpdateCategoryRequest>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Add a redundant service setup to satisfy the Strict Mock (will not be called)
        _cookbookServiceMock
            .Setup(s => s.ModifyCategoryAsync(id, It.IsAny<Category>()))
            .ReturnsAsync(new Category());

        // ACT & ASSERT
        await Assert.ThrowsAsync<ValidationException>(() => _sut.Update(validatorMock.Object, id, _mockUpdateRequest));

        // Verify service was NOT called
        _cookbookServiceMock.Verify(s => s.ModifyCategoryAsync(It.IsAny<int>(), It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task Update_WhenCategoryDoesNotExist_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        var validatorMock = Mock.Of<IValidator<UpdateCategoryRequest>>();

        // Mock service to throw when resource is not found
        _cookbookServiceMock
            .Setup(s => s.ModifyCategoryAsync(badId, It.IsAny<Category>()))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Category)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() =>
            _sut.Update(validatorMock, badId, _mockUpdateRequest));
        _cookbookServiceMock.Verify(s => s.ModifyCategoryAsync(badId, It.IsAny<Category>()), Times.Once);
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
        _cookbookServiceMock.Verify(s => s.DeleteCategoryAsync(id), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenCategoryDoesNotExist_Should_Throw_ResourceNotFoundException()
    {
        // ARRANGE
        const int badId = 99;
        // Mock service to throw when the resource is not found
        _cookbookServiceMock.Setup(s => s.DeleteCategoryAsync(badId))
            .ThrowsAsync(new ResourceNotFoundException(typeof(Category)));

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.Delete(badId));
        _cookbookServiceMock.Verify(s => s.DeleteCategoryAsync(badId), Times.Once);
    }

    #endregion
}