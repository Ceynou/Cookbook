using Cookbook.SharedModels.Entities;

namespace Cookbook.Data.Repositories;

public class IngredientRepository : IIngredientRepository
{
    public Task<IEnumerable<Ingredient>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Ingredient?> GetByAsync(int key)
    {
        throw new NotImplementedException();
    }

    public Task<Ingredient> CreateAsync(Ingredient entity)
    {
        throw new NotImplementedException();
    }

    public Task<Ingredient> ModifyAsync(Ingredient entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int key)
    {
        throw new NotImplementedException();
    }
}