using Cookbook.Data.Repositories;
using Cookbook.SharedModels.Domain.Contracts.Requests;
using Cookbook.SharedModels.Entities;
using Cookbook.SharedModels.Mappers;

namespace Cookbook.Core;

public class AccessService(IUserRepository userRepository, IPasswordHasher passwordHasher) : IAccessService
{
    public async Task<User> SignInAsync(User user)
    {
        var dbUser = await userRepository.GetByAsync(user);
        if (dbUser == null)
            throw new Exception("Invalid credentials");
        return passwordHasher.VerifyPassword(user.PasswordHash, dbUser.PasswordHash) ? dbUser : throw new Exception("Invalid credentials");
    }
}