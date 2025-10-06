using Cookbook.Data.Repositories;
using Cookbook.SharedData;
using Cookbook.SharedData.Entities;

namespace Cookbook.Core;

public class AccessService(IUserRepository userRepository, IPasswordHasher passwordHasher) : IAccessService
{
    public async Task<User> SignUpAsync(User user)
    {
        user.PasswordHash = passwordHasher.HashPassword(user.PasswordHash);
        var dbUser = await userRepository.CreateAsync(user);
        return dbUser;
    }

    public async Task<User> SignInAsync(User user)
    {
        var dbUser = await userRepository.GetByUsernameAsync(user.Username);
        if (dbUser == null)
            throw new ResourceNotFoundException(typeof(User), nameof(user.Username), user.Username);
        return passwordHasher.VerifyPassword(user.PasswordHash, dbUser.PasswordHash)
            ? dbUser
            : throw new Exception("Invalid credentials");
    }
}