using Cookbook.SharedData.Entities;

namespace Cookbook.Data.Repositories;

public interface IRecipeRepository : IGenericReadRepository<int, Recipe>, IGenericWriteRepository<int, Recipe>
{
}