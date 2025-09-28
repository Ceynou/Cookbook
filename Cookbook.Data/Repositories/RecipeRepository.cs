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

    public async Task<Recipe> CreateAsync(Recipe entity)
    {
        context.Recipes.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<Recipe> ModifyAsync(Recipe entity)
    {
        context.Recipes.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int key)
    {
        var res = await context.Recipes
            .Where(r => r.RecipeId == key).ExecuteDeleteAsync();
        return res == 1;
    }
    
}