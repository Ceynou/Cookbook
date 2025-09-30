namespace Cookbook.Data.Repositories;

public interface IGenericReadRepository<in TKey, TEntity>
{
    /// <summary>
    ///     Fetches data from the database and creates an <see cref="IEnumerable{TEntity}" />.
    /// </summary>
    /// <returns>
    ///     An <see cref="IEnumerable{TEntity}" /> that contains keys and values from <paramref name="source" /> and uses
    ///     default comparer for the key type.
    /// </returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    ///     Creates a <see cref="IEnumerable{TEntity}" /> of the entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the values from elements of <paramref name="source" /></typeparam>
    /// <param name="key">Primary key of the entity</param>
    /// <returns>
    ///     A <see cref="TEntity" /> that contains keys and values from <paramref name="source" /> and uses default
    ///     comparer for the key type.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is a null reference.</exception>
    /// <exception cref="ArgumentException"><paramref name="source" /> contains one or more duplicate keys.</exception>
    Task<TEntity?> GetByAsync(TKey key);
}