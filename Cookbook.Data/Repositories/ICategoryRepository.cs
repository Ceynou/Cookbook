using Cookbook.SharedData.Entities;

namespace Cookbook.Data.Repositories;

public interface ICategoryRepository : IGenericReadRepository<int, Category>, IGenericWriteRepository<int, Category>
{
}