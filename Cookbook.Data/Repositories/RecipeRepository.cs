using Cookbook.SharedData.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Data.Repositories;

public class RecipeRepository(CookbookContext context) : IRecipeRepository
{
    private readonly CookbookContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        return await _context.Recipes
            .Include(r => r.Creator)
            .Include(r => r.Steps)
            .Include(r => r.RecipesIngredients)
            .Include(r => r.RecipesCategories)
            .Include(r => r.Reviews)
            .ThenInclude(rw => rw.Reviewer)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Recipe?> GetByAsync(int key)
    {
        return await _context.Recipes
            .Where(r => r.RecipeId == key)
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
    }

    public async Task<Recipe> CreateAsync(Recipe entity)
    {
        _context.Recipes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Recipe?> ModifyAsync(Recipe entity)
    {
        _context.Recipes.Update(entity);
        var res = await _context.SaveChangesAsync();
        return res != 0 ? entity : null;
    }

    public async Task<bool> DeleteAsync(int key)
    {
        var res = await _context.Recipes
            .Where(r => r.RecipeId == key).ExecuteDeleteAsync();
        return res != 0;
    }
}