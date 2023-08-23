using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using Squares.Infrastructure.Persistence.Configuration;

namespace Squares.Infrastructure.Multitenancy;
public class TenantDbContext : EFCoreStoreDbContext<AppTenant>
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppTenant>().ToTable("Tenant", SchemaNames.MultiTenancy);
    }
}