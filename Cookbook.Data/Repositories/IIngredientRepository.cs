using Cookbook.SharedData.Entities;

namespace Cookbook.Data.Repositories;

public interface IIngredientRepository : IGenericReadRepository<int, Ingredient>,
    IGenericWriteRepository<int, Ingredient>
{
}