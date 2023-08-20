using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Squares.Application.Common.Events;
using Squares.Application.Common.Interfaces;
using Squares.Domain.Catalog;
using Squares.Infrastructure.Persistence.Configuration;

namespace Squares.Infrastructure.Persistence.Context;
public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, dbSettings, events)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}