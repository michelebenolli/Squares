using Ardalis.Specification.EntityFrameworkCore;
using Squares.Application.Common.Persistence;
using Squares.Infrastructure.Persistence.Context;

namespace Squares.Infrastructure.Persistence.Repository;

public class ApplicationDbRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class
{
    public ApplicationDbRepository(DatabaseContext dbContext)
        : base(dbContext)
    {
    }
}