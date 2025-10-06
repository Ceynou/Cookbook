using Cookbook.SharedData.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Data.Repositories;

public class CategoryRepository(CookbookContext context) : ICategoryRepository
{
    private readonly CookbookContext _context = context ?? throw new ArgumentNullException(nameof(context));
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .ToListAsync();
    }

    public async Task<Category?> GetByAsync(int key)
    {
        return await _context.Categories
            .FindAsync(key);
    }

    public async Task<Category> CreateAsync(Category entity)
    {
        _context.Categories.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Category?> ModifyAsync(Category entity)
    {
        _context.Categories.Update(entity);
        var res = await _context.SaveChangesAsync();
        return res != 0 ? entity : null;
    }

    public async Task<bool> DeleteAsync(int key)
    {
        var res = await _context.Categories
            .Where(c => c.CategoryId == key).ExecuteDeleteAsync();
        return res != 0;
    }
}