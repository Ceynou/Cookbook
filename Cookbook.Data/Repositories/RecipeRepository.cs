using Cookbook.Data.Interfaces;
using Cookbook.SharedModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Data.Repositories;

public class RecipeRepository(CookbookContext context) : IRecipeRepository
{
    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        return await context.Recipes
            .Include(r => r.Creator)
            .Include(r => r.Steps)
            .Include(r => r.RecipesIngredients)
            .Include(r => r.Categories)
            .Include(r => r.Reviews)
            .ThenInclude(rw => rw.Reviewer)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Recipe?> GetByAsync(int key)
    {
        return await context.Recipes
            .Where(r => r.RecipeId == key)
            .Include(r => r.Creator)
            .Include(r => r.Steps)
            .Include(r => r.RecipesIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.Categories)
            .Include(r => r.Reviews)
            .ThenInclude(rw => rw.Reviewer)
            .AsNoTracking()
            .AsSplitQuery()
            .SingleOrDefaultAsync();
    }

    public async Task<Recipe> CreateAsync(Recipe recipe)
    {
        context.Recipes.Add(recipe);
        await context.SaveChangesAsync();
        return recipe;
    }

    public async Task<Recipe> ModifyAsync(Recipe recipe)
    {
        context.Recipes.Update(recipe);
        await context.SaveChangesAsync();
        return recipe;
    }

    public async Task<bool> DeleteAsync(int key)
    {
        // TODO choose the best way to delete
        var recipe = await context.Recipes.SingleOrDefaultAsync(r => r.RecipeId == key);
        if (recipe == null)
        {
            return false;
        }
        context.Recipes.Where(r => r.RecipeId == key).ExecuteDeleteAsync();
        await context.SaveChangesAsync();
        return true;
    }
    
}