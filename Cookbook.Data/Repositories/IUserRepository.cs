using Cookbook.SharedData.Entities;

namespace Cookbook.Data.Repositories;

public interface IUserRepository : IGenericReadRepository<int, User>, IGenericWriteRepository<int, User>
{
    Task<User?> GetByUsernameAsync(string username);
}