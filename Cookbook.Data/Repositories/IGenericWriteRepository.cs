namespace Cookbook.Data
{
    public interface IGenericWriteRepository<TKey, TEntity>
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> ModifyAsync(TEntity entity);
        Task<bool> DeleteAsync(TKey key);
    }
}
