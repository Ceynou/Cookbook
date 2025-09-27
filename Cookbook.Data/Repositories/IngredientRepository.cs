using Cookbook.SharedModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Data.Repositories;

public class IngredientRepository(CookbookContext context) : IIngredientRepository
{
    public async Task<IEnumerable<Ingredient>> GetAllAsync()
    {
        return await context.Ingredients.ToListAsync();
    }

    public async Task<Ingredient?> GetByAsync(int key)
    {
        return await context.Ingredients.FindAsync(key);
    }

    public async Task<Ingredient> CreateAsync(Ingredient entity)
    {
        context.Ingredients.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<Ingredient> ModifyAsync(Ingredient entity)
    {
        context.Ingredients.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int key)
    {
        // TODO Consider using ExecuteDeleteAsync(); instead
        var entity = await GetByAsync(key);
        if (entity == null) return false;
        context.Ingredients.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}