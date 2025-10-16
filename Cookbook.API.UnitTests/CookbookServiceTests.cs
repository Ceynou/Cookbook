using System.Security.Claims;
using Cookbook.Core;
using Cookbook.Infrastructure;
using Cookbook.SharedData.Entities;
using Cookbook.SharedData.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Cookbook.API.UnitTests;

public class CookbookServiceTests : IDisposable
{
    private const short CategoryId = 1;
    private const short NonExistingCategoryId = 0;
    private const short IngredientId = 1;
    private const short NonExistingIngredientId = 0;
    private readonly CookbookContext _context;
    private readonly CookbookService _cookbookService;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    public CookbookServiceTests()
    {
        var options = new DbContextOptionsBuilder<CookbookContext>()
            .UseInMemoryDatabase($"CookbookTestDb_{Guid.NewGuid()}")
            .Options;

        _context = new CookbookContext(options);
        // Remove EnsureDeleted/EnsureCreated - not needed with unique GUID
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _cookbookService = new CookbookService(_mockHttpContextAccessor.Object, _context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    #region Recipe Tests

    [Fact]
    public async Task GetAllRecipesAsync_WithExistingRecipes_ReturnsAllRecipes()
    {
        // Arrange
        var user = new User { Username = "testuser", Email = "test@test.com", PasswordHash = "hash" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var recipe1 = new Recipe
            { Title = "Recipe 1", CreatorId = user.UserId, Difficulty = 1, ImagePath = "nothing.png" };
        var recipe2 = new Recipe
            { Title = "Recipe 2", CreatorId = user.UserId, Difficulty = 2, ImagePath = "nothing.png" };
        _context.Recipes.AddRange(recipe1, recipe2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cookbookService.GetAllRecipesAsync();

        // Assert
        Assert.NotNull(result);
        var recipesList = result.ToList();
        Assert.Equal(2, recipesList.Count);
        Assert.Contains(recipesList, r => r.Title == "Recipe 1");
        Assert.Contains(recipesList, r => r.Title == "Recipe 2");
    }

    [Fact]
    public async Task GetAllRecipesAsync_WithNoRecipes_ThrowsResourceNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _cookbookService.GetAllRecipesAsync());
    }

    [Fact]
    public async Task GetRecipeByAsync_WithExistingId_ReturnsRecipe()
    {
        // Arrange
        var user = new User { Username = "testuser", Email = "test@test.com", PasswordHash = "hash" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var recipe = new Recipe
            { Title = "Test Recipe", CreatorId = user.UserId, Difficulty = 1, ImagePath = "nothing.png" };
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cookbookService.GetRecipeByAsync(recipe.RecipeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Recipe", result.Title);
    }

    [Fact]
    public async Task GetRecipeByAsync_WithNonExistingId_ThrowsResourceNotFoundException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ResourceNotFoundException>(() => _cookbookService.GetRecipeByAsync(0));

        Assert.Contains("0", exception.Message);
    }

    [Fact]
    public async Task CreateRecipeAsync_WithValidRecipe_CreatesAndReturnsRecipe()
    {
        // Arrange
        var user = new User { Username = "testuser", Email = "test@test.com", PasswordHash = "hash" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, "testuser")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = claimsPrincipal };
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        var recipe = new Recipe
        {
            Title = "New Recipe",
            Difficulty = 2,
            CookingDuration = TimeSpan.FromMinutes(30),
            PreparationDuration = TimeSpan.FromMinutes(15),
            ImagePath = "image.png"
        };

        // Act
        var result = await _cookbookService.CreateRecipeAsync(recipe);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Recipe", result.Title);
        Assert.Equal(user.UserId, result.CreatorId);

        var savedRecipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Title == "New Recipe");
        Assert.NotNull(savedRecipe);
    }

    // [Fact]
    // public async Task CreateRecipeAsync_WithoutHttpContext_CreatesRecipeWithoutCreatorId()
    // {
    //     // Arrange
    //     _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns((HttpContext)null);
    //     
    //     var recipe = new Recipe
    //     {
    //         Title = "New Recipe",
    //         Difficulty = 2,
    //         CookingDuration = TimeSpan.FromMinutes(30),
    //         PreparationDuration = TimeSpan.FromMinutes(15),
    //         ImagePath = "image.png"
    //     };
    //
    //     // Act
    //     var result = await _cookbookService.CreateRecipeAsync(recipe);
    //
    //     // Assert
    //     Assert.NotNull(result);
    //     Assert.Equal("New Recipe", result.Title);
    //     Assert.Equal(0, result.CreatorId); // Default value when no user context
    // }

    [Fact]
    public async Task ModifyRecipeAsync_WithExistingRecipe_UpdatesAndReturnsRecipe()
    {
        // Arrange
        var user = new User { Username = "testuser", Email = "test@test.com", PasswordHash = "hash" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var recipe = new Recipe
            { Title = "Original Title", CreatorId = user.UserId, Difficulty = 1, ImagePath = "nothing.png" };
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();
        _context.Entry(recipe).State = EntityState.Detached;

        var updatedRecipe = new Recipe
        {
            Title = "Updated Title",
            CreatorId = user.UserId,
            Difficulty = 3,
            CookingDuration = TimeSpan.FromMinutes(45),
            ImagePath = "nothing.png"
        };

        // Act
        var result = await _cookbookService.ModifyRecipeAsync(recipe.RecipeId, updatedRecipe);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(recipe.RecipeId, result.RecipeId);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal(3, result.Difficulty);
    }

    [Fact]
    public async Task ModifyRecipeAsync_WithNonExistingRecipe_ThrowsResourceNotFoundException()
    {
        // Arrange
        var recipe = new Recipe { Title = "Test", Difficulty = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _cookbookService.ModifyRecipeAsync(0, recipe));
    }

    [Fact]
    public async Task DeleteRecipeAsync_WithExistingRecipe_RemovesRecipe()
    {
        // Arrange
        var user = new User { Username = "testuser", Email = "test@test.com", PasswordHash = "hash" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var recipe = new Recipe
            { Title = "To Delete", CreatorId = user.UserId, Difficulty = 1, ImagePath = "nothing.png" };
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        var recipeId = recipe.RecipeId;

        // Act
        await _cookbookService.DeleteRecipeAsync(recipeId);

        // Assert
        var deletedRecipe = await _context.Recipes.FindAsync(recipeId);
        Assert.Null(deletedRecipe);
    }

    [Fact]
    public async Task DeleteRecipeAsync_WithNonExistingRecipe_ThrowsResourceNotFoundException()
    {
        const short recipeId = 0;
        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _cookbookService.DeleteRecipeAsync(recipeId));
    }

    #endregion

    #region Ingredient Tests

    [Fact]
    public async Task GetAllIngredientsAsync_WithExistingIngredients_ReturnsAllIngredients()
    {
        // Arrange
        var ingredient1 = new Ingredient { Name = "Salt" };
        var ingredient2 = new Ingredient { Name = "Pepper" };
        _context.Ingredients.AddRange(ingredient1, ingredient2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cookbookService.GetAllIngredientsAsync();

        // Assert
        Assert.NotNull(result);
        var ingredientsList = result.ToList();
        Assert.Equal(2, ingredientsList.Count);
        Assert.Contains(ingredientsList, i => i.Name == "Salt");
        Assert.Contains(ingredientsList, i => i.Name == "Pepper");
    }

    [Fact]
    public async Task GetAllIngredientsAsync_WithNoIngredients_ThrowsResourceNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _cookbookService.GetAllIngredientsAsync());
    }

    [Fact]
    public async Task GetIngredientByAsync_WithExistingId_ReturnsIngredient()
    {
        // Arrange
        var ingredient = new Ingredient { Name = "Sugar" };
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cookbookService.GetIngredientByAsync(ingredient.IngredientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Sugar", result.Name);
    }

    [Fact]
    public async Task GetIngredientByAsync_WithNonExistingId_ThrowsResourceNotFoundException()
    {
        const short ingredientId = 0;
        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _cookbookService.GetIngredientByAsync(ingredientId));
    }

    [Fact]
    public async Task CreateIngredientAsync_WithValidIngredient_CreatesAndReturnsIngredient()
    {
        // Arrange
        var ingredient = new Ingredient { Name = "Flour" };

        // Act
        var result = await _cookbookService.CreateIngredientAsync(ingredient);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Flour", result.Name);

        var savedIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == "Flour");
        Assert.NotNull(savedIngredient);
    }

    [Fact]
    public async Task ModifyIngredientAsync_WithExistingIngredient_UpdatesAndReturnsIngredient()
    {
        // Arrange
        var ingredient = new Ingredient { Name = "Original Name" };
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();
        _context.Entry(ingredient).State = EntityState.Detached;

        var updatedIngredient = new Ingredient { Name = "Updated Name" };

        // Act
        var result = await _cookbookService.ModifyIngredientAsync(ingredient.IngredientId, updatedIngredient);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ingredient.IngredientId, result.IngredientId);
        Assert.Equal("Updated Name", result.Name);
    }

    [Fact]
    public async Task ModifyIngredientAsync_WithNonExistingIngredient_ThrowsResourceNotFoundException()
    {
        // Arrange
        var ingredient = new Ingredient { Name = "Test" };

        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() =>
            _cookbookService.ModifyIngredientAsync(0, ingredient));
    }

    [Fact]
    public async Task DeleteIngredientAsync_WithExistingIngredient_RemovesIngredient()
    {
        // Arrange
        var ingredient = new Ingredient { Name = "To Delete" };
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();

        var ingredientId = ingredient.IngredientId;

        // Act
        await _cookbookService.DeleteIngredientAsync(ingredientId);

        // Assert
        var deletedIngredient = await _context.Ingredients.FindAsync(ingredientId);
        Assert.Null(deletedIngredient);
    }

    [Fact]
    public async Task DeleteIngredientAsync_WithNonExistingIngredient_ThrowsResourceNotFoundException()
    {
        const short ingredientId = 0;
        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _cookbookService.DeleteIngredientAsync(ingredientId));
    }

    #endregion

    #region Category Tests

    [Fact]
    public async Task GetAllCategoriesAsync_WithExistingCategories_ReturnsAllCategories()
    {
        // Arrange
        var category1 = new Category { Name = "Dessert" };
        var category2 = new Category { Name = "Main Course" };
        _context.Categories.AddRange(category1, category2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cookbookService.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        var categoriesList = result.ToList();
        Assert.Equal(2, categoriesList.Count);
        Assert.Contains(categoriesList, c => c.Name == "Dessert");
        Assert.Contains(categoriesList, c => c.Name == "Main Course");
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WithNoCategories_ThrowsResourceNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _cookbookService.GetAllCategoriesAsync());
    }

    [Fact]
    public async Task GetCategoryByAsync_WithExistingId_ReturnsCategory()
    {
        // Arrange
        var category = new Category { Name = "Appetizer" };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cookbookService.GetCategoryByAsync(category.CategoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Appetizer", result.Name);
    }

    [Fact]
    public async Task GetCategoryByAsync_WithNonExistingId_ThrowsResourceNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _cookbookService.GetCategoryByAsync(0));
    }

    [Fact]
    public async Task CreateCategoryAsync_WithValidCategory_CreatesAndReturnsCategory()
    {
        // Arrange
        var category = new Category { Name = "Breakfast" };

        // Act
        var result = await _cookbookService.CreateCategoryAsync(category);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Breakfast", result.Name);

        var savedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Breakfast");
        Assert.NotNull(savedCategory);
    }

    [Fact]
    public async Task ModifyCategoryAsync_WithExistingCategory_UpdatesAndReturnsCategory()
    {
        // Arrange
        var category = new Category { Name = "Original Name" };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        _context.Entry(category).State = EntityState.Detached;

        var updatedCategory = new Category { Name = "Updated Name" };

        // Act
        var result = await _cookbookService.ModifyCategoryAsync(category.CategoryId, updatedCategory);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(category.CategoryId, result.CategoryId);
        Assert.Equal("Updated Name", result.Name);
    }

    [Fact]
    public async Task ModifyCategoryAsync_WithNonExistingCategory_ThrowsResourceNotFoundException()
    {
        // Arrange
        var category = new Category { Name = "Test" };

        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _cookbookService.ModifyCategoryAsync(0, category));
    }

    [Fact]
    public async Task DeleteCategoryAsync_WithExistingCategory_RemovesCategory()
    {
        // Arrange
        var category = new Category { Name = "To Delete" };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var categoryId = category.CategoryId;

        // Act
        await _cookbookService.DeleteCategoryAsync(categoryId);

        // Assert
        var deletedCategory = await _context.Categories.FindAsync(categoryId);
        Assert.Null(deletedCategory);
    }

    [Fact]
    public async Task DeleteCategoryAsync_WithNonExistingCategory_ThrowsResourceNotFoundException()
    {
        const short categoryId = 0;
        // Act & Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _cookbookService.DeleteCategoryAsync(categoryId));
    }

    #endregion

    #region Integration Tests

    [Fact]
    public async Task CreateRecipeAsync_WithMultipleUsers_AssignsCorrectCreator()
    {
        // Arrange
        var user1 = new User { Username = "user1", Email = "user1@test.com", PasswordHash = "hash1" };
        var user2 = new User { Username = "user2", Email = "user2@test.com", PasswordHash = "hash2" };
        _context.Users.AddRange(user1, user2);
        await _context.SaveChangesAsync();

        // User 1 creates a recipe
        var claims1 = new[] { new Claim(ClaimTypes.NameIdentifier, user1.UserId.ToString()) };
        var identity1 = new ClaimsIdentity(claims1, "TestAuthType");
        var httpContext1 = new DefaultHttpContext { User = new ClaimsPrincipal(identity1) };
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext1);

        var recipe1 = new Recipe { Title = "User1 Recipe", Difficulty = 1, ImagePath = "nothing1.png" };
        var result1 = await _cookbookService.CreateRecipeAsync(recipe1);

        // User 2 creates a recipe
        var claims2 = new[] { new Claim(ClaimTypes.NameIdentifier, user2.UserId.ToString()) };
        var identity2 = new ClaimsIdentity(claims2, "TestAuthType");
        var httpContext2 = new DefaultHttpContext { User = new ClaimsPrincipal(identity2) };
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext2);

        var recipe2 = new Recipe { Title = "User2 Recipe", Difficulty = 2, ImagePath = "nothing2.png" };
        var result2 = await _cookbookService.CreateRecipeAsync(recipe2);

        // Assert
        Assert.Equal(user1.UserId, result1.CreatorId);
        Assert.Equal(user2.UserId, result2.CreatorId);
    }

    [Fact]
    public async Task GetRecipeByAsync_WithRelatedEntities_LoadsAllNavigationProperties()
    {
        // Arrange
        var user = new User { Username = "chef", Email = "chef@test.com", PasswordHash = "hash" };
        var ingredient = new Ingredient { Name = "Tomato" };
        var category = new Category { Name = "Italian" };

        _context.Users.Add(user);
        _context.Ingredients.Add(ingredient);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var recipe = new Recipe
        {
            Title = "Pasta",
            CreatorId = user.UserId,
            Difficulty = 2,
            ImagePath = "nothing.png"
        };
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        var step = new Step
        {
            RecipeId = recipe.RecipeId,
            StepNumber = 1,
            Instruction = "Boil water",
            Duration = TimeSpan.FromMinutes(10)
        };
        var recipeIngredient = new RecipesIngredient
        {
            RecipeId = recipe.RecipeId,
            IngredientId = ingredient.IngredientId,
            Quantity = 2,
            Unit = "pieces"
        };
        var recipeCategory = new RecipesCategory
        {
            RecipeId = recipe.RecipeId,
            CategoryId = category.CategoryId
        };

        _context.Steps.Add(step);
        _context.RecipesIngredients.Add(recipeIngredient);
        _context.RecipesCategories.Add(recipeCategory);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cookbookService.GetRecipeByAsync(recipe.RecipeId);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Creator);
        Assert.Equal("chef", result.Creator.Username);
        Assert.NotEmpty(result.Steps);
        Assert.NotEmpty(result.RecipesIngredients);
        Assert.NotEmpty(result.RecipesCategories);
    }

    #endregion
}