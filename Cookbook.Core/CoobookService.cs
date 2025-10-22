using System.Security.Claims;
using Cookbook.Infrastructure;
using Cookbook.SharedData.Entities;
using Cookbook.SharedData.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Core;

public class CookbookService(
    IHttpContextAccessor httpContextAccessor,
    CookbookContext context) : ICookbookService
{
    #region CRUD for Recipes

    public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
    {
        var recipes = await context.Recipes
            .Include(r => r.Creator)
            .Include(r => r.Steps)
            .Include(r => r.RecipesIngredients)
            .Include(r => r.RecipesCategories)
            .Include(r => r.Reviews)
            .ThenInclude(rw => rw.Reviewer)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        return recipes.Count == 0 ? throw new ResourceNotFoundException(recipes.GetType()) : recipes;
    }

    public async Task<Recipe> GetRecipeByAsync(int id)
    {
        var recipe = await context.Recipes
            .Where(r => r.RecipeId == id)
            .Include(r => r.Creator)
            .Include(r => r.Steps)
            .Include(r => r.RecipesIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.RecipesCategories)
            .ThenInclude(r => r.Category)
            .Include(r => r.Reviews)
            .ThenInclude(rw => rw.Reviewer)
            .AsSplitQuery()
            .SingleOrDefaultAsync();

        return recipe ?? throw new ResourceNotFoundException(typeof(Recipe), nameof(recipe.RecipeId), id);
    }

    public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (int.TryParse(userIdClaim, out var creatorId))
            recipe.CreatorId = creatorId;

        context.Recipes.Add(recipe);
        await context.SaveChangesAsync();
        return recipe;
    }

    public async Task<Recipe> ModifyRecipeAsync(int id, Recipe recipe)
    {
        var existingRecipe = await GetRecipeByAsync(id);

        recipe.RecipeId = id;

        context.RecipesIngredients.RemoveRange(existingRecipe.RecipesIngredients);
        context.RecipesCategories.RemoveRange(existingRecipe.RecipesCategories);
        context.Steps.RemoveRange(existingRecipe.Steps);

        context.Entry(existingRecipe).CurrentValues.SetValues(recipe);

        existingRecipe.RecipesIngredients = recipe.RecipesIngredients;
        existingRecipe.RecipesCategories = recipe.RecipesCategories;
        existingRecipe.Steps = recipe.Steps;

        await context.SaveChangesAsync();

        return recipe;
    }

    public async Task DeleteRecipeAsync(int id)
    {
        var recipe = await context.Recipes.FindAsync(id);
        if (recipe == null)
            throw new ResourceNotFoundException(typeof(Recipe), nameof(recipe.RecipeId), id);
        context.Recipes.Remove(recipe);
        await context.SaveChangesAsync();
    }

    #endregion

    #region CRUD for Ingredients

    public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
    {
        var ingredients = await context.Ingredients.ToListAsync();
        return ingredients.Count == 0
            ? throw new ResourceNotFoundException(typeof(IEnumerable<Ingredient>))
            : ingredients;
    }

    public async Task<Ingredient> GetIngredientByAsync(short id)
    {
        var ingredient = await context.Ingredients.FindAsync(id);
        return ingredient ?? throw new ResourceNotFoundException(typeof(Ingredient), nameof(Ingredient), id);
    }

    public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient)
    {
        context.Ingredients.Add(ingredient);
        await context.SaveChangesAsync();
        return ingredient;
    }

    public async Task<Ingredient> ModifyIngredientAsync(short id, Ingredient ingredient)
    {
        var ingredientExists = await context.Ingredients.AnyAsync(i => i.IngredientId == id);
        if (!ingredientExists)
            throw new ResourceNotFoundException(typeof(Ingredient), nameof(Ingredient.IngredientId), id);

        ingredient.IngredientId = id;
        context.Ingredients.Update(ingredient);
        await context.SaveChangesAsync();
        return ingredient;
    }

    public async Task DeleteIngredientAsync(short id)
    {
        var ingredient = await context.Ingredients.FindAsync(id);
        if (ingredient == null)
            throw new ResourceNotFoundException(typeof(Ingredient), nameof(Ingredient), id);
        context.Ingredients.Remove(ingredient);
        await context.SaveChangesAsync();
    }

    #endregion

    #region CRUD for Categories

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        var categories = await context.Categories.ToListAsync();
        return categories.Count == 0 ? throw new ResourceNotFoundException(categories.GetType()) : categories;
    }

    public async Task<Category> GetCategoryByAsync(short id)
    {
        var category = await context.Categories.FindAsync(id);
        return category ?? throw new ResourceNotFoundException(typeof(Category), nameof(Category), id);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> ModifyCategoryAsync(short id, Category category)
    {
        var categoryExists = await context.Categories.AnyAsync(c => c.CategoryId == id);
        if (!categoryExists)
            throw new ResourceNotFoundException(typeof(Category), nameof(Category.CategoryId), id);

        category.CategoryId = id;
        context.Update(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task DeleteCategoryAsync(short id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
            throw new ResourceNotFoundException(typeof(Category), nameof(Category), id);
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
    }

    #endregion
}