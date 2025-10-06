using Cookbook.SharedData.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Data.Repositories;

public class UserRepository(CookbookContext context) : IUserRepository
{
    private readonly CookbookContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .ToListAsync();
    }

    public async Task<User?> GetByAsync(int key)
    {
        return await _context.Users
            .FindAsync(key);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> CreateAsync(User entity)
    {
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<User?> ModifyAsync(User entity)
    {
        _context.Users.Update(entity);
        var res = await _context.SaveChangesAsync();
        return res != 0 ? entity : null;
    }

    public async Task<bool> DeleteAsync(int key)
    {
        var res = await _context.Users
            .Where(u => u.UserId == key)
            .ExecuteDeleteAsync();
        return res != 0;
    }
}