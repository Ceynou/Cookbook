using Cookbook.SharedModels.Entities;

namespace Cookbook.Data.Repositories;

public interface ICategoryRepository : IGenericReadRepository<int, Category>, IGenericWriteRepository<int, Category>
{
    
}