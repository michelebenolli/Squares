using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Squares.Domain.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Squares.Infrastructure.Persistence.Repository;

public abstract class RepositoryBase<T> : Application.Common.Persistence.IRepositoryBase<T>
    where T : class
{
    private readonly DbContext _db;
    private readonly ISpecificationEvaluator _evaluator;

    protected RepositoryBase(DbContext db)
        : this(db, SpecificationEvaluator.Default)
    {
    }

    protected RepositoryBase(DbContext db, ISpecificationEvaluator evaluator)
    {
        _db = db;
        _evaluator = evaluator;
    }

    public virtual void Insert(T entity)
    {
        _db.Set<T>().Add(entity);
    }

    public virtual void Insert(IEnumerable<T>? entities)
    {
        if (entities?.Any() == true)
        {
            _db.Set<T>().AddRange(entities);
        }
    }

    public virtual void Update(T entity)
    {
        _db.Set<T>().Update(entity);
    }

    public virtual void Update(IEnumerable<T>? entities)
    {
        if (entities?.Any() == true)
        {
            _db.Set<T>().UpdateRange(entities);
        }
    }

    public virtual void Delete(T entity)
    {
        _db.Set<T>().Remove(entity);
    }

    public virtual void Delete(IEnumerable<T>? entities)
    {
        if (entities?.Any() == true)
        {
            _db.Set<T>().RemoveRange(entities);
        }
    }

    public virtual void Sort<TS>(IEnumerable<TS>? entities, List<int> ids)
        where TS : T, ISortable
    {
        if (entities?.Any() == true)
        {
            int order = 0;

            foreach (var entity in entities.OrderBy(x => ids?.IndexOf(x.Id)))
            {
                if (entity.Order != order)
                {
                    entity.Order = order;
                    Update(entity);
                }

                order++;
            }
        }
    }

    public virtual async Task<int> SaveAsync(CancellationToken token = default)
    {
        return await _db.SaveChangesAsync(token);
    }

    public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken token = default)
        where TId : notnull
    {
        return await _db.Set<T>().FindAsync(new object[] { id }, cancellationToken: token);
    }

    public virtual async Task<T?> GetAsync(ISpecification<T> specification, CancellationToken token = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(token);
    }

    public virtual async Task<TResult?> GetAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken token = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(token);
    }

    public virtual async Task<List<T>> ListAsync(CancellationToken token = default)
    {
        return await _db.Set<T>().ToListAsync(token);
    }

    public virtual async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken token = default)
    {
        var queryResult = await ApplySpecification(specification).ToListAsync(token);

        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
    }

    public virtual async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken token = default)
    {
        var queryResult = await ApplySpecification(specification).ToListAsync(token);

        return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
    }

    public virtual async Task<IPagedList<T>> PagedListAsync(
        int pageNumber, int pageSize, CancellationToken token = default)
    {
        return await _db.Set<T>().ToPagedListAsync(pageNumber, pageSize, token);
    }

    public virtual async Task<IPagedList<T>> PagedListAsync(
        ISpecification<T> specification, int pageNumber, int pageSize, CancellationToken token = default)
    {
        return await ApplySpecification(specification).ToPagedListAsync(pageNumber, pageSize, token);
    }

    public virtual async Task<IPagedList<TResult>> PagedListAsync<TResult>(
        ISpecification<T, TResult> specification, int pageNumber, int pageSize, CancellationToken token = default)
    {
        return await ApplySpecification(specification).ToPagedListAsync(pageNumber, pageSize, token);
    }

    public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken token = default)
    {
        return await ApplySpecification(specification, true).CountAsync(token);
    }

    public virtual async Task<int> CountAsync(CancellationToken token = default)
    {
        return await _db.Set<T>().CountAsync(token);
    }

    public virtual async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken token = default)
    {
        return await ApplySpecification(specification, true).AnyAsync(token);
    }

    public virtual async Task<bool> AnyAsync(CancellationToken token = default)
    {
        return await _db.Set<T>().AnyAsync(token);
    }

    protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
    {
        return _evaluator.GetQuery(_db.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
    }

    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return _evaluator.GetQuery(_db.Set<T>().AsQueryable(), specification);
    }
}