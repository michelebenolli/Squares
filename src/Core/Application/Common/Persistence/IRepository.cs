namespace Squares.Application.Common.Persistence;

/// <summary>
/// The regular read/write repository.
/// </summary>
public interface IRepository<T> : IRepositoryBase<T>
    where T : class
{
}

/// <summary>
/// The read-only repository.
/// </summary>
public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class
{
}

/// <summary>
/// A read/write repository that also adds events before adding, updating or deleting entities.
/// </summary>
public interface IRepositoryWithEvents<T> : IRepositoryBase<T>
    where T : class, IEntity
{
}