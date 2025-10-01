using Cookbook.Data.Repositories;
using Cookbook.SharedData;
using Cookbook.SharedData.Entities;
using Microsoft.AspNetCore.Http;

namespace Cookbook.Core;

public class CookbookService(
    IHttpContextAccessor httpContextAccessor,
    IRecipeRepository recipeRepository,
    ICategoryRepository categoryRepository,
    IIngredientRepository ingredientRepository,
    IUserRepository userRepository) : ICookbookService
{
    #region CRUD for Recipes

    public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
    {
        var recipes = await recipeRepository.GetAllAsync();

        if (!recipes.Any())
            throw new ResourceNotFoundException(typeof(IEnumerable<Recipe>));

        return recipes;
    }

    public async Task<Recipe> GetRecipeByAsync(int id)
    {
        var recipe = await recipeRepository.GetByAsync(id);

        if (recipe == null)
            throw new ResourceNotFoundException(typeof(Recipe), nameof(recipe.RecipeId), id);
        return recipe;
    }

    public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userIdClaim = user?.FindFirst("user_id")?.Value;
        if (int.TryParse(userIdClaim, out var creatorId))
            recipe.CreatorId = creatorId;
        else
            // Handle case where User ID is not present or invalid
            throw new Exception("Creator ID claim missing or invalid.");


        var entity = await recipeRepository.CreateAsync(recipe);
        if (entity == null)
            throw new ResourceNotFoundException(typeof(Recipe), nameof(recipe.RecipeId), recipe.RecipeId);
        return entity;
    }

    public async Task<Recipe> ModifyRecipeAsync(int id, Recipe recipe)
    {
        var res = await recipeRepository.ModifyAsync(recipe);
        if (res == null)
            throw new ResourceNotFoundException(typeof(Recipe), nameof(recipe.RecipeId), recipe.RecipeId);
        return res;
    }

    public async Task DeleteRecipeAsync(int id)
    {
        var res = await recipeRepository.DeleteAsync(id);
        if (!res)
            throw new ResourceNotFoundException(typeof(Recipe), nameof(Recipe), id);
    }

    #endregion

    #region CRUD for Users

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await userRepository.GetAllAsync();
    }

    public async Task<User> GetUserByAsync(int id)
    {
        var res = await userRepository.GetByAsync(id);
        if (res == null)
            throw new ResourceNotFoundException(typeof(User), nameof(User), id);
        return res;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var res = await userRepository.GetByUsernameAsync(username);
        if (res == null)
            throw new ResourceNotFoundException(typeof(User), nameof(User), username);
        return res;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var res = await userRepository.CreateAsync(user);
        if (res == null)
            throw new ResourceNotFoundException(typeof(User), nameof(User), user.Username);
        return res;
    }

    public async Task<User> ModifyUserAsync(int id, User user)
    {
        var res = await userRepository.ModifyAsync(user);
        if (res == null)
            throw new ResourceNotFoundException(typeof(User), nameof(User), user.Username);
        return res;
    }

    public async Task DeleteUserAsync(int id)
    {
        var res = await userRepository.DeleteAsync(id);
        if (!res)
            throw new ResourceNotFoundException(typeof(User), nameof(User), id);
    }

    #endregion

    #region CRUD for Ingredients

    public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
    {
        var res = await ingredientRepository.GetAllAsync();
        if (!res.Any())
            throw new ResourceNotFoundException(typeof(IEnumerable<Ingredient>));
        return res;
    }

    public async Task<Ingredient> GetIngredientByAsync(int id)
    {
        var res = await ingredientRepository.GetByAsync(id);
        if (res == null)
            throw new ResourceNotFoundException(typeof(Ingredient), nameof(Ingredient), id);
        return res;
    }

    public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient)
    {
        var res = await ingredientRepository.CreateAsync(ingredient);
        if (res == null)
            throw new ResourceNotFoundException(typeof(Ingredient), nameof(Ingredient), ingredient.Name);
        return res;
    }

    public async Task<Ingredient> ModifyIngredientAsync(int id, Ingredient ingredient)
    {
        var res = await ingredientRepository.ModifyAsync(ingredient);
        if (res == null)
            throw new ResourceNotFoundException(typeof(Ingredient), nameof(Ingredient), ingredient.Name);
        return res;
    }

    public async Task DeleteIngredientAsync(int id)
    {
        var res = await ingredientRepository.DeleteAsync(id);
        if (!res)
            throw new ResourceNotFoundException(typeof(Ingredient), nameof(Ingredient), id);
    }

    #endregion

    #region CRUD for Categories

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        var res = await categoryRepository.GetAllAsync();
        if (!res.Any())
            throw new ResourceNotFoundException(typeof(IEnumerable<Category>));
        return res;
    }

    public async Task<Category> GetCategoryByAsync(int id)
    {
        var res = await categoryRepository.GetByAsync(id);
        if (res == null)
            throw new ResourceNotFoundException(typeof(Category), nameof(Category), id);
        return res;
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        var res = await categoryRepository.CreateAsync(category);
        if (res == null)
            throw new ResourceNotFoundException(typeof(Category), nameof(Category), category.Name);
        return res;
    }

    public async Task<Category> ModifyCategoryAsync(int id, Category category)
    {
        var res = await categoryRepository.ModifyAsync(category);
        if (res == null)
            throw new ResourceNotFoundException(typeof(Category), nameof(Category), category.Name);
        return res;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var res = await categoryRepository.DeleteAsync(id);
        if (!res)
            throw new ResourceNotFoundException(typeof(Category), nameof(Category), id);
    }

    #endregion
}