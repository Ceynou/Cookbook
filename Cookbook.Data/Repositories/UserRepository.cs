using Cookbook.SharedData.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Data.Repositories;

public class UserRepository(CookbookContext context) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> GetByAsync(int key)
    {
        return await context.Users
            .FindAsync(key);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> CreateAsync(User entity)
    {
        context.Users.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<User?> ModifyAsync(User entity)
    {
        context.Users.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int key)
    {
        var res = await context.Users
            .Where(u => u.UserId == key)
            .ExecuteDeleteAsync();
        return res == 1;
    }
}