namespace Squares.Application.Common.Persistence;

public interface IRepositoryBase<T> : IReadRepositoryBase<T>
    where T : class
{
    void Insert(T entity);
    void Insert(IEnumerable<T>? entities);
    void Update(T entity);
    void Update(IEnumerable<T>? entities);
    void Delete(T entity);
    void Delete(IEnumerable<T>? entities);
    Task<int> SaveAsync(CancellationToken token = default);
    void Sort<TS>(IEnumerable<TS>? entities, List<int> ids)
        where TS : T, ISortable;
}
