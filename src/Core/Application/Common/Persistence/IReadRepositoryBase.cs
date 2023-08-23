namespace Squares.Application.Common.Persistence;

public interface IReadRepositoryBase<T>
    where T : class
{
    Task<T?> GetByIdAsync<TId>(TId id, CancellationToken token = default)
        where TId : notnull;
    Task<T?> GetAsync(ISpecification<T> specification, CancellationToken token = default);
    Task<TResult?> GetAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken token = default);
    Task<List<T>> ListAsync(CancellationToken token = default);
    Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken token = default);
    Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken token = default);
    Task<IPagedList<T>> PagedListAsync(int pageNumber, int pageSize, CancellationToken token = default);
    Task<IPagedList<T>> PagedListAsync(ISpecification<T> specification, int pageNumber, int pageSize, CancellationToken token = default);
    Task<IPagedList<TResult>> PagedListAsync<TResult>(ISpecification<T, TResult> specification, int pageNumber, int pageSize, CancellationToken token = default);
    Task<int> CountAsync(ISpecification<T> specification, CancellationToken token = default);
    Task<int> CountAsync(CancellationToken token = default);
    Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken token = default);
    Task<bool> AnyAsync(CancellationToken token = default);
}
