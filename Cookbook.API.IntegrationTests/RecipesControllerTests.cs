using System.Net;
using System.Net.Http.Json;
using Cookbook.API.IntegrationTests.Fixtures;
using Cookbook.SharedData.Contracts.Requests;
using Cookbook.SharedData.Contracts.Responses;
using Xunit;

namespace Cookbook.API.IntegrationTests;

public class RecipesControllerTests(APiWebApplicationFactory webApi) : IntegrationTest(webApi)
{
    private readonly CreateRecipeCategoryRequest _createRecipeCategoryRequest = new()
    {
        CategoryId = 1
    };

    private readonly CreateRecipeIngredientRequest _createRecipesIngredientRequest = new()
    {
        IngredientId = 1,
        Quantity = 1000
    };

    private readonly CreateStepRequest _createStepRequest = new()
    {
        StepNumber = 1,
        Instruction = "Something to do.",
        Duration = TimeSpan.FromMinutes(10),
        IsCooking = false
    };

    private readonly UpdateRecipeCategoryRequest _updateRecipeCategoryRequest = new()
    {
        CategoryId = 1
    };

    private readonly UpdateRecipeIngredientRequest _updateRecipeIngredientRequest = new()
    {
        IngredientId = 1,
        Quantity = 1000
    };

    private readonly UpdateStepRequest _updateStepRequest = new()
    {
        StepNumber = 1,
        Instruction = "Something to do.",
        Duration = TimeSpan.FromMinutes(10),
        IsCooking = false
    };

    #region Integration Workflow Tests

    [Fact]
    public async Task CompleteWorkflow_CreateReadUpdateDelete_Success()
    {
        // Arrange
        await SignIn("admin", "admin");

        // Create
        var createRequest = new CreateRecipeRequest
        {
            Title = "Workflow Test Recipe",
            Difficulty = 2,
            ImagePath = "workflow.jpg",
            Ingredients = [_createRecipesIngredientRequest],
            Categories = [_createRecipeCategoryRequest],
            Steps = [_createStepRequest]
        };
        var createResponse = await HttpClient.PostAsJsonAsync("/api/cookbook/Recipes", createRequest);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var createdRecipe = await createResponse.Content.ReadFromJsonAsync<RecipeResponse>();
        Assert.NotNull(createdRecipe);

        // Read
        var readResponse = await HttpClient.GetAsync($"/api/cookbook/Recipes/{createdRecipe.RecipeId}");
        Assert.Equal(HttpStatusCode.OK, readResponse.StatusCode);
        var readRecipe = await readResponse.Content.ReadFromJsonAsync<RecipeResponse>();
        Assert.NotNull(readRecipe);
        Assert.Equal(createRequest.Title, readRecipe.Title);

        // Update
        var updateRequest = new UpdateRecipeRequest
        {
            Title = "Updated Workflow Recipe",
            Difficulty = 4,
            ImagePath = "workflow_updated.jpg",
            Ingredients = [_updateRecipeIngredientRequest],
            Categories = [_updateRecipeCategoryRequest],
            Steps = [_updateStepRequest]
        };
        var updateResponse =
            await HttpClient.PutAsJsonAsync($"/api/cookbook/Recipes/{createdRecipe.RecipeId}", updateRequest);
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        var updatedRecipe = await updateResponse.Content.ReadFromJsonAsync<RecipeResponse>();
        Assert.NotNull(updatedRecipe);
        Assert.Equal(updateRequest.Title, updatedRecipe.Title);

        // Delete
        var deleteResponse = await HttpClient.DeleteAsync($"/api/cookbook/Recipes/{createdRecipe.RecipeId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Verify deletion
        var verifyResponse = await HttpClient.GetAsync($"/api/cookbook/Recipes/{createdRecipe.RecipeId}");
        Assert.Equal(HttpStatusCode.NotFound, verifyResponse.StatusCode);
    }

    #endregion


    #region GetAll Tests

    [Fact]
    public async Task GetAll_WhenAuthenticated_ReturnsOkWithRecipes()
    {
        // Arrange
        await SignIn("admin", "admin");

        // Act
        var response = await HttpClient.GetAsync("/api/cookbook/Recipes");
        var recipes = await response.Content.ReadFromJsonAsync<List<RecipeResponse>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(recipes);
        Assert.NotEmpty(recipes);
        Assert.True(recipes.Count > 0);
    }

    [Fact]
    public async Task GetAll_WhenNotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        SignOut();

        // Act
        var response = await HttpClient.GetAsync("/api/cookbook/Recipes");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_AsRegularUser_ReturnsOkWithRecipes()
    {
        // Arrange
        await SignIn("user", "user");

        // Act
        var response = await HttpClient.GetAsync("/api/cookbook/Recipes");
        var recipes = await response.Content.ReadFromJsonAsync<List<RecipeResponse>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(recipes);
        Assert.NotEmpty(recipes);
    }

    #endregion

    #region GetBy Tests

    [Fact]
    public async Task GetBy_WhenRecipeExists_ReturnsOkWithRecipe()
    {
        // Arrange
        await SignIn("admin", "admin");
        const int recipeId = 1;

        // Act
        var response = await HttpClient.GetAsync($"/api/cookbook/Recipes/{recipeId}");
        var recipe = await response.Content.ReadFromJsonAsync<RecipeResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(recipe);
        Assert.Equal(recipeId, recipe.RecipeId);
        Assert.False(string.IsNullOrEmpty(recipe.Title));
    }

    [Fact]
    public async Task GetBy_WhenRecipeDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await SignIn("admin", "admin");
        const int nonExistentRecipeId = 9999;

        // Act
        var response = await HttpClient.GetAsync($"/api/cookbook/Recipes/{nonExistentRecipeId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetBy_WhenNotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        SignOut();
        const int recipeId = 1;

        // Act
        var response = await HttpClient.GetAsync($"/api/cookbook/Recipes/{recipeId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion

    #region GetFullBy Tests

    [Fact]
    public async Task GetFullBy_WhenRecipeExists_ReturnsOkWithDetailedRecipe()
    {
        // Arrange
        await SignIn("admin", "admin");
        const int recipeId = 1;

        // Act
        var response = await HttpClient.GetAsync($"/api/cookbook/Recipes/{recipeId}/full");
        var recipe = await response.Content.ReadFromJsonAsync<RecipeDetailResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(recipe);
        Assert.Equal(recipeId, recipe.RecipeId);
        Assert.False(string.IsNullOrEmpty(recipe.Title));
        Assert.NotNull(recipe.Steps);
        Assert.NotNull(recipe.Ingredients);
        Assert.NotNull(recipe.Categories);
    }

    [Fact]
    public async Task GetFullBy_WhenRecipeDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await SignIn("admin", "admin");
        const int nonExistentRecipeId = 9999;

        // Act
        var response = await HttpClient.GetAsync($"/api/cookbook/Recipes/{nonExistentRecipeId}/full");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedWithRecipe()
    {
        // Arrange
        await SignIn("admin", "admin");
        var request = new CreateRecipeRequest
        {
            Title = "Test Recipe",
            Difficulty = 2,
            ImagePath = "test.jpg",
            Ingredients = [_createRecipesIngredientRequest],
            Categories = [_createRecipeCategoryRequest],
            Steps = [_createStepRequest]
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/cookbook/Recipes", request);
        var createdRecipe = await response.Content.ReadFromJsonAsync<RecipeResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdRecipe);
        Assert.Equal(request.Title, createdRecipe.Title);
        Assert.Equal(request.Difficulty, createdRecipe.Difficulty);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Create_WhenNotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        SignOut();
        var request = new CreateRecipeRequest
        {
            Title = "Test Recipe",
            Difficulty = 2,
            ImagePath = "test.jpg",
            Ingredients = [_createRecipesIngredientRequest],
            Categories = [_createRecipeCategoryRequest],
            Steps = [_createStepRequest]
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/cookbook/Recipes", request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        await SignIn("admin", "admin");
        var request = new CreateRecipeRequest
        {
            Title = "", // Invalid: empty title
            Difficulty = 2,
            ImagePath = "nothing.png",
            Ingredients = [_createRecipesIngredientRequest],
            Categories = [_createRecipeCategoryRequest],
            Steps = [_createStepRequest]
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/cookbook/Recipes", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_AsRegularUser_ReturnsForbidden()
    {
        // Arrange
        await SignIn("user", "user");
        var request = new CreateRecipeRequest
        {
            Title = "User Recipe",
            Difficulty = 1,
            ImagePath = "user_recipe.jpg",
            Ingredients = [_createRecipesIngredientRequest],
            Categories = [_createRecipeCategoryRequest],
            Steps = [_createStepRequest]
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/cookbook/Recipes", request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Modify_WithValidData_ReturnsOkWithUpdatedRecipe()
    {
        // Arrange
        await SignIn("admin", "admin");
        const int recipeId = 1;
        var request = new UpdateRecipeRequest
        {
            Title = "Updated Recipe Title",
            Difficulty = 3,
            ImagePath = "updated.jpg",
            Ingredients = [_updateRecipeIngredientRequest],
            Categories = [_updateRecipeCategoryRequest],
            Steps = [_updateStepRequest]
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/api/cookbook/Recipes/{recipeId}", request);
        var updatedRecipe = await response.Content.ReadFromJsonAsync<RecipeResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(updatedRecipe);
        Assert.Equal(recipeId, updatedRecipe.RecipeId);
        Assert.Equal(request.Title, updatedRecipe.Title);
        Assert.Equal(request.Difficulty, updatedRecipe.Difficulty);
    }

    [Fact]
    public async Task Modify_WhenRecipeDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await SignIn("admin", "admin");
        const int nonExistentRecipeId = 9999;
        var request = new UpdateRecipeRequest
        {
            Title = "Updated Recipe Title",
            Difficulty = 3,
            ImagePath = "updated.jpg",
            Ingredients = [_updateRecipeIngredientRequest],
            Categories = [_updateRecipeCategoryRequest],
            Steps = [_updateStepRequest]
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/api/cookbook/Recipes/{nonExistentRecipeId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Modify_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        await SignIn("admin", "admin");
        const int recipeId = 1;
        var request = new UpdateRecipeRequest
        {
            Title = "", // Invalid: empty title
            Difficulty = 3,
            Ingredients = [_updateRecipeIngredientRequest],
            Categories = [_updateRecipeCategoryRequest],
            Steps = [_updateStepRequest]
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/api/cookbook/Recipes/{recipeId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Modify_WhenNotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        SignOut();
        const int recipeId = 1;
        var request = new UpdateRecipeRequest
        {
            Title = "Updated Recipe Title",
            Difficulty = 3,
            ImagePath = "updated.jpg",
            Ingredients = [_updateRecipeIngredientRequest],
            Categories = [_updateRecipeCategoryRequest],
            Steps = [_updateStepRequest]
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/api/cookbook/Recipes/{recipeId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_WhenRecipeExists_ReturnsNoContent()
    {
        // Arrange
        await SignIn("admin", "admin");

        // Create a recipe to delete
        var createRequest = new CreateRecipeRequest
        {
            Title = "Recipe To Delete",
            Difficulty = 1,
            ImagePath = "delete_me.jpg",
            Ingredients = [_createRecipesIngredientRequest],
            Categories = [_createRecipeCategoryRequest],
            Steps = [_createStepRequest]
        };
        var createResponse = await HttpClient.PostAsJsonAsync("/api/cookbook/Recipes", createRequest);
        var createdRecipe = await createResponse.Content.ReadFromJsonAsync<RecipeResponse>();

        // Act
        var deleteResponse = await HttpClient.DeleteAsync($"/api/cookbook/Recipes/{createdRecipe!.RecipeId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Verify recipe is deleted
        var getResponse = await HttpClient.GetAsync($"/api/cookbook/Recipes/{createdRecipe.RecipeId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenRecipeDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await SignIn("admin", "admin");
        const int nonExistentRecipeId = 9999;

        // Act
        var response = await HttpClient.DeleteAsync($"/api/cookbook/Recipes/{nonExistentRecipeId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenNotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        SignOut();
        const int recipeId = 1;

        // Act
        var response = await HttpClient.DeleteAsync($"/api/cookbook/Recipes/{recipeId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion
}