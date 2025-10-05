using System.Security.Claims;
using Cookbook.Core;
using Cookbook.Data.Repositories;
using Cookbook.SharedData;
using Cookbook.SharedData.Entities;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Cookbook.API.Tests;

public class CookbookServiceUnitTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly CookbookService _sut; // System Under Test
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public CookbookServiceUnitTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();

        // Initialize the service with all mocks
        _sut = new CookbookService(
            _httpContextAccessorMock.Object,
            _recipeRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _ingredientRepositoryMock.Object,
            _userRepositoryMock.Object
        );
    }

    #region CRUD for Recipes

    [Fact]
    public async Task GetAllRecipesAsync_WhenRecipesExist_ReturnsAllRecipes()
    {
        // ARRANGE
        var recipes = new List<Recipe> { new() { RecipeId = 1 }, new() { RecipeId = 2 } };
        _recipeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(recipes);

        // ACT
        var result = await _sut.GetAllRecipesAsync();

        // ASSERT
        Assert.Equal(2, result.Count());
        Assert.Equal(1, result.First().RecipeId);
    }

    [Fact]
    public async Task GetAllRecipesAsync_WhenNoRecipesExist_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        _recipeRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Recipe>());

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetAllRecipesAsync());
    }

    [Fact]
    public async Task GetRecipeByAsync_WhenRecipeExists_ReturnsRecipe()
    {
        // ARRANGE
        const int recipeId = 10;
        var expectedRecipe = new Recipe { RecipeId = recipeId };
        _recipeRepositoryMock.Setup(r => r.GetByAsync(recipeId)).ReturnsAsync(expectedRecipe);

        // ACT
        var result = await _sut.GetRecipeByAsync(recipeId);

        // ASSERT
        Assert.Equal(recipeId, result.RecipeId);
    }

    [Fact]
    public async Task GetRecipeByAsync_WhenRecipeDoesNotExist_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        const int recipeId = 99;
        _recipeRepositoryMock.Setup(r => r.GetByAsync(recipeId)).ReturnsAsync((Recipe)null!);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetRecipeByAsync(recipeId));
    }

    [Fact]
    public async Task CreateRecipeAsync_WithValidUserId_SetsCreatorIdAndReturnsRecipe()
    {
        // ARRANGE
        const int expectedCreatorId = 5;
        var recipeToCreate = new Recipe { Title = "New Recipe" };
        var createdRecipe = new Recipe { RecipeId = 1, CreatorId = expectedCreatorId };

        // Mock the authenticated user with a "user_id" claim
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, expectedCreatorId.ToString()) };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

        // Mock repository creation to return the final entity
        _recipeRepositoryMock
            .Setup(r => r.CreateAsync(It.Is<Recipe>(recipe => recipe.CreatorId == expectedCreatorId)))
            .ReturnsAsync(createdRecipe);

        // ACT
        var result = await _sut.CreateRecipeAsync(recipeToCreate);

        // ASSERT
        Assert.Equal(expectedCreatorId, result.CreatorId);
        _recipeRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Recipe>()), Times.Once);
    }

    [Fact]
    public async Task CreateRecipeAsync_WithoutUserIdClaim_ThrowsException()
    {
        // ARRANGE
        var recipeToCreate = new Recipe { Title = "New Recipe" };

        // Mock the authenticated user WITHOUT a "user_id" claim
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

        // ACT & ASSERT
        var exception = await Assert.ThrowsAsync<Exception>(() => _sut.CreateRecipeAsync(recipeToCreate));
        Assert.Equal("Creator ID claim missing or invalid.", exception.Message);

        // Verify that the repository method was NOT called
        _recipeRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Recipe>()), Times.Never);
    }

    [Fact]
    public async Task ModifyRecipeAsync_WhenSuccessful_ReturnsModifiedRecipe()
    {
        // ARRANGE
        const int recipeId = 10;
        var recipeToModify = new Recipe { RecipeId = recipeId, Title = "Updated Title" };
        _recipeRepositoryMock.Setup(r => r.ModifyAsync(recipeToModify)).ReturnsAsync(recipeToModify);

        // ACT
        var result = await _sut.ModifyRecipeAsync(recipeId, recipeToModify);

        // ASSERT
        Assert.Equal("Updated Title", result.Title);
        _recipeRepositoryMock.Verify(r => r.ModifyAsync(recipeToModify), Times.Once);
    }

    [Fact]
    public async Task ModifyRecipeAsync_WhenNotFound_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        const int recipeId = 99;
        var recipeToModify = new Recipe { RecipeId = recipeId, Title = "Update" };
        _recipeRepositoryMock.Setup(r => r.ModifyAsync(recipeToModify)).ReturnsAsync((Recipe)null!);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.ModifyRecipeAsync(recipeId, recipeToModify));
    }

    [Fact]
    public async Task DeleteRecipeAsync_WhenSuccessful_CompletesWithoutException()
    {
        // ARRANGE
        const int recipeId = 10;
        _recipeRepositoryMock.Setup(r => r.DeleteAsync(recipeId)).ReturnsAsync(true);

        // ACT
        await _sut.DeleteRecipeAsync(recipeId);

        // ASSERT
        _recipeRepositoryMock.Verify(r => r.DeleteAsync(recipeId), Times.Once);
    }

    [Fact]
    public async Task DeleteRecipeAsync_WhenNotFound_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        const int recipeId = 99;
        _recipeRepositoryMock.Setup(r => r.DeleteAsync(recipeId)).ReturnsAsync(false);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.DeleteRecipeAsync(recipeId));
    }

    #endregion

    #region CRUD for Users

    // Note: User CRUD methods are typically outside the main Cookbook service, 
    // but they are included here as per the ICookbookService interface.

    [Fact]
    public async Task GetAllUsersAsync_WhenUsersExist_ReturnsAllUsers()
    {
        // ARRANGE
        var users = new List<User> { new() { UserId = 1 }, new() { UserId = 2 } };
        _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        // ACT
        var result = await _sut.GetAllUsersAsync();

        // ASSERT
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllUsersAsync_WhenNoUsersExist_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetAllUsersAsync());
    }

    [Fact]
    public async Task GetUserByAsync_WhenUserExists_ReturnsUser()
    {
        // ARRANGE
        const int userId = 10;
        var expectedUser = new User { UserId = userId };
        _userRepositoryMock.Setup(r => r.GetByAsync(userId)).ReturnsAsync(expectedUser);

        // ACT
        var result = await _sut.GetUserByAsync(userId);

        // ASSERT
        Assert.Equal(userId, result.UserId);
    }

    [Fact]
    public async Task GetUserByAsync_WhenUserDoesNotExist_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        const int userId = 99;
        _userRepositoryMock.Setup(r => r.GetByAsync(userId)).ReturnsAsync((User)null!);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetUserByAsync(userId));
    }

    [Fact]
    public async Task GetUserByUsernameAsync_WhenUserExists_ReturnsUser()
    {
        // ARRANGE
        const string username = "testuser";
        var expectedUser = new User { Username = username };
        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(username)).ReturnsAsync(expectedUser);

        // ACT
        var result = await _sut.GetUserByUsernameAsync(username);

        // ASSERT
        Assert.Equal(username, result.Username);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_WhenUserDoesNotExist_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        const string username = "nonexistent";
        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(username)).ReturnsAsync((User)null!);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetUserByUsernameAsync(username));
    }

    [Fact]
    public async Task CreateUserAsync_WhenSuccessful_ReturnsCreatedUser()
    {
        // ARRANGE
        var userToCreate = new User { Username = "newuser" };
        var createdUser = new User { UserId = 1, Username = "newuser" };
        _userRepositoryMock.Setup(r => r.CreateAsync(userToCreate)).ReturnsAsync(createdUser);

        // ACT
        var result = await _sut.CreateUserAsync(userToCreate);

        // ASSERT
        Assert.Equal(1, result.UserId);
        _userRepositoryMock.Verify(r => r.CreateAsync(userToCreate), Times.Once);
    }

    [Fact]
    public async Task ModifyUserAsync_WhenSuccessful_ReturnsModifiedUser()
    {
        // ARRANGE
        const int userId = 1;
        var userToModify = new User { UserId = userId, Username = "updated" };
        _userRepositoryMock.Setup(r => r.ModifyAsync(userToModify)).ReturnsAsync(userToModify);

        // ACT
        var result = await _sut.ModifyUserAsync(userId, userToModify);

        // ASSERT
        Assert.Equal("updated", result.Username);
        _userRepositoryMock.Verify(r => r.ModifyAsync(userToModify), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenSuccessful_CompletesWithoutException()
    {
        // ARRANGE
        const int userId = 1;
        _userRepositoryMock.Setup(r => r.DeleteAsync(userId)).ReturnsAsync(true);

        // ACT
        await _sut.DeleteUserAsync(userId);

        // ASSERT
        _userRepositoryMock.Verify(r => r.DeleteAsync(userId), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenNotFound_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        const int userId = 99;
        _userRepositoryMock.Setup(r => r.DeleteAsync(userId)).ReturnsAsync(false);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.DeleteUserAsync(userId));
    }

    #endregion

    #region CRUD for Categories

    [Fact]
    public async Task GetAllCategoriesAsync_WhenCategoriesExist_ReturnsAllCategories()
    {
        // ARRANGE
        var categories = new List<Category> { new() { CategoryId = 1 } };
        _categoryRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

        // ACT
        var result = await _sut.GetAllCategoriesAsync();

        // ASSERT
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenNoCategoriesExist_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        _categoryRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Category>());

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetAllCategoriesAsync());
    }

    [Fact]
    public async Task GetCategoryByAsync_WhenCategoryExists_ReturnsCategory()
    {
        // ARRANGE
        const int categoryId = 10;
        var expectedCategory = new Category { CategoryId = categoryId };
        _categoryRepositoryMock.Setup(r => r.GetByAsync(categoryId)).ReturnsAsync(expectedCategory);

        // ACT
        var result = await _sut.GetCategoryByAsync(categoryId);

        // ASSERT
        Assert.Equal(categoryId, result.CategoryId);
    }

    [Fact]
    public async Task GetCategoryByAsync_WhenCategoryDoesNotExist_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        const int categoryId = 99;
        _categoryRepositoryMock.Setup(r => r.GetByAsync(categoryId)).ReturnsAsync((Category)null!);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetCategoryByAsync(categoryId));
    }

    [Fact]
    public async Task CreateCategoryAsync_WhenSuccessful_ReturnsCreatedCategory()
    {
        // ARRANGE
        var categoryToCreate = new Category { Name = "Dessert" };
        var createdCategory = new Category { CategoryId = 1, Name = "Dessert" };
        _categoryRepositoryMock.Setup(r => r.CreateAsync(categoryToCreate)).ReturnsAsync(createdCategory);

        // ACT
        var result = await _sut.CreateCategoryAsync(categoryToCreate);

        // ASSERT
        Assert.Equal(1, result.CategoryId);
        _categoryRepositoryMock.Verify(r => r.CreateAsync(categoryToCreate), Times.Once);
    }

    [Fact]
    public async Task ModifyCategoryAsync_WhenSuccessful_ReturnsModifiedCategory()
    {
        // ARRANGE
        const int categoryId = 1;
        var categoryToModify = new Category { CategoryId = categoryId, Name = "Lunch" };
        _categoryRepositoryMock.Setup(r => r.ModifyAsync(categoryToModify)).ReturnsAsync(categoryToModify);

        // ACT
        var result = await _sut.ModifyCategoryAsync(categoryId, categoryToModify);

        // ASSERT
        Assert.Equal("Lunch", result.Name);
        _categoryRepositoryMock.Verify(r => r.ModifyAsync(categoryToModify), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenSuccessful_CompletesWithoutException()
    {
        // ARRANGE
        const int categoryId = 1;
        _categoryRepositoryMock.Setup(r => r.DeleteAsync(categoryId)).ReturnsAsync(true);

        // ACT
        await _sut.DeleteCategoryAsync(categoryId);

        // ASSERT
        _categoryRepositoryMock.Verify(r => r.DeleteAsync(categoryId), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenNotFound_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        const int categoryId = 99;
        _categoryRepositoryMock.Setup(r => r.DeleteAsync(categoryId)).ReturnsAsync(false);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.DeleteCategoryAsync(categoryId));
    }

    #endregion

    #region CRUD for Ingredients

    [Fact]
    public async Task GetAllIngredientsAsync_WhenIngredientsExist_ReturnsAllIngredients()
    {
        // ARRANGE
        var ingredients = new List<Ingredient> { new() { IngredientId = 1 } };
        _ingredientRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(ingredients);

        // ACT
        var result = await _sut.GetAllIngredientsAsync();

        // ASSERT
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAllIngredientsAsync_WhenNoIngredientsExist_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        _ingredientRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Ingredient>());

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetAllIngredientsAsync());
    }

    [Fact]
    public async Task GetIngredientByAsync_WhenIngredientExists_ReturnsIngredient()
    {
        // ARRANGE
        const int ingredientId = 10;
        var expectedIngredient = new Ingredient { IngredientId = ingredientId };
        _ingredientRepositoryMock.Setup(r => r.GetByAsync(ingredientId)).ReturnsAsync(expectedIngredient);

        // ACT
        var result = await _sut.GetIngredientByAsync(ingredientId);

        // ASSERT
        Assert.Equal(ingredientId, result.IngredientId);
    }

    [Fact]
    public async Task GetIngredientByAsync_WhenIngredientDoesNotExist_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        const int ingredientId = 99;
        _ingredientRepositoryMock.Setup(r => r.GetByAsync(ingredientId)).ReturnsAsync((Ingredient)null!);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.GetIngredientByAsync(ingredientId));
    }

    [Fact]
    public async Task CreateIngredientAsync_WhenSuccessful_ReturnsCreatedIngredient()
    {
        // ARRANGE
        var ingredientToCreate = new Ingredient { Name = "Salt" };
        var createdIngredient = new Ingredient { IngredientId = 1, Name = "Salt" };
        _ingredientRepositoryMock.Setup(r => r.CreateAsync(ingredientToCreate)).ReturnsAsync(createdIngredient);

        // ACT
        var result = await _sut.CreateIngredientAsync(ingredientToCreate);

        // ASSERT
        Assert.Equal(1, result.IngredientId);
        _ingredientRepositoryMock.Verify(r => r.CreateAsync(ingredientToCreate), Times.Once);
    }

    [Fact]
    public async Task ModifyIngredientAsync_WhenSuccessful_ReturnsModifiedIngredient()
    {
        // ARRANGE
        const int ingredientId = 1;
        var ingredientToModify = new Ingredient { IngredientId = ingredientId, Name = "Pepper" };
        _ingredientRepositoryMock.Setup(r => r.ModifyAsync(ingredientToModify)).ReturnsAsync(ingredientToModify);

        // ACT
        var result = await _sut.ModifyIngredientAsync(ingredientId, ingredientToModify);

        // ASSERT
        Assert.Equal("Pepper", result.Name);
        _ingredientRepositoryMock.Verify(r => r.ModifyAsync(ingredientToModify), Times.Once);
    }

    [Fact]
    public async Task DeleteIngredientAsync_WhenSuccessful_CompletesWithoutException()
    {
        // ARRANGE
        const int ingredientId = 1;
        _ingredientRepositoryMock.Setup(r => r.DeleteAsync(ingredientId)).ReturnsAsync(true);

        // ACT
        await _sut.DeleteIngredientAsync(ingredientId);

        // ASSERT
        _ingredientRepositoryMock.Verify(r => r.DeleteAsync(ingredientId), Times.Once);
    }

    [Fact]
    public async Task DeleteIngredientAsync_WhenNotFound_ThrowsResourceNotFoundException()
    {
        // ARRANGE
        const int ingredientId = 99;
        _ingredientRepositoryMock.Setup(r => r.DeleteAsync(ingredientId)).ReturnsAsync(false);

        // ACT & ASSERT
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _sut.DeleteIngredientAsync(ingredientId));
    }

    #endregion
}