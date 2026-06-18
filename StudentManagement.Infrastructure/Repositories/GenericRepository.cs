using StudentManagement.Domain.Interfaces;

namespace StudentManagement.Infrastructure.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
{
    protected readonly List<T> _store = [];
    private int _nextId = 1;

    public Task<IEnumerable<T>> GetAllAsync()
        => Task.FromResult<IEnumerable<T>>([.. _store]);

    public Task<T?> GetByIdAsync(int id)
        => Task.FromResult(_store.FirstOrDefault(x => x.Id == id));

    public Task AddAsync(T entity)
    {
        entity.Id = _nextId++;
        _store.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(T entity)
    {
        var index = _store.FindIndex(x => x.Id == entity.Id);
        if (index == -1)
            throw new KeyNotFoundException($"Entity with Id {entity.Id} not found.");

        _store[index] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var entity = _store.FirstOrDefault(x => x.Id == id)
            ?? throw new KeyNotFoundException($"Entity with Id {id} not found.");

        _store.Remove(entity);
        return Task.CompletedTask;
    }
}
