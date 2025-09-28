using Cookbook.SharedModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Data.Repositories;

public class CategoryRepository(CookbookContext context) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await context.Categories.ToListAsync();
    }

    public async Task<Category?> GetByAsync(int key)
    {
        return await context.Categories.FindAsync(key);
    }

    public async Task<Category> CreateAsync(Category entity)
    {
        context.Categories.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<Category> ModifyAsync(Category entity)
    {
        context.Categories.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int key)
    {
        var res = await context.Categories
            .Where(c => c.CategoryId == key).ExecuteDeleteAsync();
        return res == 1;
    }
}