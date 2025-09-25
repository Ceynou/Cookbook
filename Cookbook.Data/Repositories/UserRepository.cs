using Cookbook.SharedModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Data.Repositories;

public class UserRepository(CookbookContext context) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users
            .ToListAsync();
    }

    public async Task<User?> GetByAsync(int key)
    {
        return await context.Users
            .Include(u => u.Recipes)
            .SingleOrDefaultAsync(u => u.UserId == key);
    }

    public async Task<User?> GetByAsync(User user)
    {
        return await context.Users
            .SingleOrDefaultAsync(u => 
                u.Email == user.Email 
                && u.Username == user.Username 
                && u.PasswordHash == user.PasswordHash);
    }

    public async Task<User> CreateAsync(User entity)
    {
        context.Users.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<User> ModifyAsync(User entity)
    {
        context.Users.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public Task<bool> DeleteAsync(int key)
    {
        throw new NotImplementedException();
    }
}