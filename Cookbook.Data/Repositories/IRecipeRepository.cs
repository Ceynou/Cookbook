using Cookbook.Data;
using Cookbook.SharedModels.Entities;

namespace Cookbook.Data.Interfaces;

public interface IRecipeRepository : IGenericReadRepository<int, Recipe>, IGenericWriteRepository<int, Recipe>
{
}