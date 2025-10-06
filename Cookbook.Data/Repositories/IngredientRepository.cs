using Cookbook.SharedData.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Data.Repositories;

public class IngredientRepository(CookbookContext context) : IIngredientRepository
{
    private readonly CookbookContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IEnumerable<Ingredient>> GetAllAsync()
    {
        return await _context.Ingredients
            .ToListAsync();
    }

    public async Task<Ingredient?> GetByAsync(int key)
    {
        return await _context.Ingredients
            .FindAsync(key);
    }

    public async Task<Ingredient> CreateAsync(Ingredient entity)
    {
        _context.Ingredients.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Ingredient?> ModifyAsync(Ingredient entity)
    {
        _context.Ingredients.Update(entity);
        var res = await _context.SaveChangesAsync();
        return res != 0 ? entity : null;
    }

    public async Task<bool> DeleteAsync(int key)
    {
        var res = await _context.Ingredients
            .Where(i => i.IngredientId == key).ExecuteDeleteAsync();
        return res != 0;
    }
}