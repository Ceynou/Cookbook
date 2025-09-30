namespace Cookbook.Data.Repositories;

public interface IGenericWriteRepository<in TKey, TEntity>
{
    Task<TEntity?> CreateAsync(TEntity entity);
    Task<TEntity?> ModifyAsync(TEntity entity);
    Task<bool> DeleteAsync(TKey key);
}