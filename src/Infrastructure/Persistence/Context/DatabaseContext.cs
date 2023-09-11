using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Squares.Application.Common.Events;
using Squares.Application.Common.Interfaces;
using Squares.Domain.Games;
using Squares.Domain.Identity;

namespace Squares.Infrastructure.Persistence.Context;
public class DatabaseContext : BaseDbContext
{
    public DatabaseContext(
        ITenantInfo currentTenant,
        DbContextOptions options,
        ICurrentUser currentUser,
        ISerializerService serializer,
        IOptions<DatabaseSettings> dbSettings,
        IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, dbSettings, events)
    {
    }

    public new DbSet<User> Users => Set<User>();
    public DbSet<Game> Games => Set<Game>();
}