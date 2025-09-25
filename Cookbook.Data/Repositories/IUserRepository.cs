using Cookbook.SharedModels.Entities;

namespace Cookbook.Data.Repositories;

public interface IUserRepository : IGenericReadRepository<int, User>, IGenericWriteRepository<int, User>
{
    Task<User?> GetByAsync(User user);
}