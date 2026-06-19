namespace StudentManagement.Domain.Interfaces;

/// <summary>
/// Generic repository contract providing basic CRUD operations.
/// </summary>
/// <typeparam name="T">Entity type that implements <see cref="IEntity"/>.</typeparam>
public interface IGenericRepository<T> where T : class, IEntity
{
    /// <summary>Retrieves all entities.</summary>
    /// <returns>A collection of all stored entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>Retrieves a single entity by its identifier.</summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The matching entity, or <c>null</c> if not found.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>Persists a new entity.</summary>
    /// <param name="entity">The entity to add.</param>
    Task AddAsync(T entity);

    /// <summary>Replaces an existing entity with updated data.</summary>
    /// <param name="entity">The entity containing updated values.</param>
    Task UpdateAsync(T entity);

    /// <summary>Removes an entity by its identifier.</summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    Task DeleteAsync(int id);
}
