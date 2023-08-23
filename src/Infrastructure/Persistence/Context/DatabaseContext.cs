using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Squares.Application.Common.Events;
using Squares.Application.Common.Interfaces;
using Squares.Domain.Identity;
using Squares.Infrastructure.Persistence.Configuration;

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
}