namespace Cookbook.Data
{
    public interface IGenericReadRepository<TKey, TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByAsync(TKey key);
    }
}
