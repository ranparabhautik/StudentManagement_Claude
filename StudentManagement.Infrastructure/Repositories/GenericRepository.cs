using StudentManagement.Domain.Interfaces;

namespace StudentManagement.Infrastructure.Repositories;

/// <summary>
/// In-memory generic repository providing basic CRUD operations for any entity type.
/// </summary>
/// <typeparam name="T">Entity type that implements <see cref="IEntity"/>.</typeparam>
public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
{
    /// <summary>In-memory data store for entities of type <typeparamref name="T"/>.</summary>
    protected readonly List<T> _store = [];
    private int _nextId = 1;

    /// <summary>Returns all stored entities.</summary>
    /// <returns>A snapshot of all entities currently in the store.</returns>
    public Task<IEnumerable<T>> GetAllAsync()
        => Task.FromResult<IEnumerable<T>>([.. _store]);

    /// <summary>Finds a single entity by its identifier.</summary>
    /// <param name="id">The entity's unique identifier.</param>
    /// <returns>The matching entity, or <c>null</c> if not found.</returns>
    public Task<T?> GetByIdAsync(int id)
        => Task.FromResult(_store.FirstOrDefault(x => x.Id == id));

    /// <summary>Assigns an auto-incremented identifier and adds the entity to the store.</summary>
    /// <param name="entity">The entity to add.</param>
    public Task AddAsync(T entity)
    {
        entity.Id = _nextId++;
        _store.Add(entity);
        return Task.CompletedTask;
    }

    /// <summary>Replaces the stored entity at the same index with the updated entity.</summary>
    /// <param name="entity">The entity with updated values.</param>
    /// <exception cref="KeyNotFoundException">Thrown when no entity with the matching identifier exists.</exception>
    public Task UpdateAsync(T entity)
    {
        var index = _store.FindIndex(x => x.Id == entity.Id);
        if (index == -1)
            throw new KeyNotFoundException($"Entity with Id {entity.Id} not found.");

        _store[index] = entity;
        return Task.CompletedTask;
    }

    /// <summary>Removes the entity with the given identifier from the store.</summary>
    /// <param name="id">The unique identifier of the entity to remove.</param>
    /// <exception cref="KeyNotFoundException">Thrown when no entity with the given identifier exists.</exception>
    public Task DeleteAsync(int id)
    {
        var entity = _store.FirstOrDefault(x => x.Id == id)
            ?? throw new KeyNotFoundException($"Entity with Id {id} not found.");

        _store.Remove(entity);
        return Task.CompletedTask;
    }
}
