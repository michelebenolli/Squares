using Ardalis.Specification;
using Squares.Application.Common.Persistence;
using Squares.Domain.Common.Contracts;
using Squares.Domain.Common.Events;
using X.PagedList;

namespace Squares.Infrastructure.Persistence.Repository;

/// <summary>
/// The repository that implements IRepositoryWithEvents.
/// Implemented as a decorator. It only augments the Add,
/// Update and Delete calls where it adds the respective
/// EntityCreated, EntityUpdated or EntityDeleted event
/// before delegating to the decorated repository.
/// </summary>
public class EventAddingRepositoryDecorator<T> : IRepositoryWithEvents<T>
    where T : class, IEntity
{
    private readonly IRepository<T> _repository;

    public EventAddingRepositoryDecorator(IRepository<T> repository)
    {
        _repository = repository;
    }

    public void Insert(T entity)
    {
        entity.DomainEvents.Add(EntityCreatedEvent.WithEntity(entity));
        _repository.Insert(entity);
    }

    public void Insert(IEnumerable<T>? entities)
    {
        if (entities?.Any() == true)
        {
            foreach (var entity in entities)
            {
                entity.DomainEvents.Add(EntityCreatedEvent.WithEntity(entity));
            }
        }

        _repository.Insert(entities);
    }

    public void Update(T entity)
    {
        entity.DomainEvents.Add(EntityUpdatedEvent.WithEntity(entity));
        _repository.Update(entity);
    }

    public void Update(IEnumerable<T>? entities)
    {
        if (entities?.Any() == true)
        {
            foreach (var entity in entities)
            {
                entity.DomainEvents.Add(EntityUpdatedEvent.WithEntity(entity));
            }
        }

        _repository.Update(entities);
    }

    public void Delete(T entity)
    {
        entity.DomainEvents.Add(EntityDeletedEvent.WithEntity(entity));
        _repository.Delete(entity);
    }

    public void Delete(IEnumerable<T>? entities)
    {
        if (entities?.Any() == true)
        {
            foreach (var entity in entities)
            {
                entity.DomainEvents.Add(EntityDeletedEvent.WithEntity(entity));
            }
        }

        _repository.Delete(entities);
    }

    public void Sort<TS>(IEnumerable<TS>? entities, List<int> ids)
        where TS : T, ISortable
    {
        if (entities?.Any() == true)
        {
            foreach (var entity in entities)
            {
                entity.DomainEvents.Add(EntitySortedEvent.WithEntity(entity));
            }
        }

        _repository.Sort(entities, ids);
    }

    // The rest of the methods are simply forwarded
    public Task<T?> GetByIdAsync<TId>(TId id, CancellationToken token = default)
        where TId : notnull =>
        _repository.GetByIdAsync(id, token);
    public Task<List<T>> ListAsync(CancellationToken token = default) =>
        _repository.ListAsync(token);
    public Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken token = default) =>
        _repository.ListAsync(specification, token);
    public Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken token = default) =>
        _repository.ListAsync(specification, token);
    public Task<T?> GetAsync(ISpecification<T> specification, CancellationToken token = default) =>
    _repository.GetAsync(specification, token);
    public Task<TResult?> GetAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken token = default) =>
        _repository.GetAsync(specification, token);
    public Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken token = default) =>
        _repository.AnyAsync(specification, token);
    public Task<bool> AnyAsync(CancellationToken token = default) =>
        _repository.AnyAsync(token);
    public Task<int> CountAsync(ISpecification<T> specification, CancellationToken token = default) =>
        _repository.CountAsync(specification, token);
    public Task<int> CountAsync(CancellationToken token = default) =>
        _repository.CountAsync(token);
    public Task<int> SaveAsync(CancellationToken token = default) =>
        _repository.SaveAsync(token);
    public Task<IPagedList<T>> PagedListAsync(
        int pageNumber, int pageSize, CancellationToken token = default) =>
        _repository.PagedListAsync(pageNumber, pageSize, token);
    public Task<IPagedList<T>> PagedListAsync(
        ISpecification<T> specification, int pageNumber, int pageSize, CancellationToken token = default) =>
        _repository.PagedListAsync(specification, pageNumber, pageSize, token);
    public Task<IPagedList<TResult>> PagedListAsync<TResult>(
        ISpecification<T, TResult> specification, int pageNumber, int pageSize, CancellationToken token = default) =>
        _repository.PagedListAsync(specification, pageNumber, pageSize, token);
}