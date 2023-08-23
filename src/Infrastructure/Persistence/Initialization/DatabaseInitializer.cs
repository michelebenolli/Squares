using Finbuckle.MultiTenant;
using Squares.Infrastructure.Multitenancy;
using Squares.Shared.Multitenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Squares.Infrastructure.Persistence.Initialization;

internal class DatabaseInitializer : IDatabaseInitializer
{
    private readonly TenantDbContext _tenantDbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        TenantDbContext tenantDbContext,
        IServiceProvider serviceProvider,
        ILogger<DatabaseInitializer> logger)
    {
        _tenantDbContext = tenantDbContext;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task InitializeDatabasesAsync(CancellationToken token)
    {
        await InitializeTenantDbAsync(token);

        foreach (var tenant in await _tenantDbContext.TenantInfo.ToListAsync(token))
        {
            await InitializeApplicationDbForTenantAsync(tenant, token);
        }
    }

    public async Task InitializeApplicationDbForTenantAsync(AppTenant tenant, CancellationToken token)
    {
        // First create a new scope
        using var scope = _serviceProvider.CreateScope();

        // Then set current tenant so the right connectionstring is used
        _serviceProvider.GetRequiredService<IMultiTenantContextAccessor>()
            .MultiTenantContext = new MultiTenantContext<AppTenant>()
            {
                TenantInfo = tenant
            };

        // Then run the initialization in the new scope
        await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
            .InitializeAsync(token);
    }

    private async Task InitializeTenantDbAsync(CancellationToken token)
    {
        if (_tenantDbContext.Database.GetPendingMigrations().Any())
        {
            _logger.LogInformation("Applying Root Migrations.");
            await _tenantDbContext.Database.MigrateAsync(token);
        }

        await SeedRootTenantAsync(token);
    }

    private async Task SeedRootTenantAsync(CancellationToken token)
    {
        if (await _tenantDbContext.TenantInfo.FindAsync(new object?[] { MultitenancyConstants.Root.Id }, cancellationToken: token) is null)
        {
            var rootTenant = new AppTenant
            {
                Id = MultitenancyConstants.Root.Id,
                Identifier = MultitenancyConstants.Root.Id,
                Name = MultitenancyConstants.Root.Name,
                AdminEmail = MultitenancyConstants.Root.EmailAddress
            };

            _tenantDbContext.TenantInfo.Add(rootTenant);
            await _tenantDbContext.SaveChangesAsync(token);
        }
    }
}