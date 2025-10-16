using Cookbook.Infrastructure;
using Cookbook.SharedData.Entities;
using Cookbook.SharedData.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Core;

public class AccessService(CookbookContext context, IPasswordHasher passwordHasher) : IAccessService
{
    public async Task<User> SignUpAsync(User user)
    {
        if (await context.Users.AnyAsync(u => u.Username == user.Username))
            throw new DuplicatePropertyException(nameof(user.Username), user.Username);

        if (await context.Users.AnyAsync(u => u.Email == user.Email))
            throw new DuplicatePropertyException(nameof(user.Email), user.Email);

        user.PasswordHash = passwordHasher.HashPassword(user.PasswordHash);
        context.Users.Add(user);

        await context.SaveChangesAsync();

        return user;
    }

    public async Task<User> SignInAsync(User user)
    {
        var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (dbUser == null)
            throw new InvalidCredentialsException();
        return passwordHasher.VerifyPassword(user.PasswordHash, dbUser.PasswordHash)
            ? dbUser
            : throw new InvalidCredentialsException();
    }
}