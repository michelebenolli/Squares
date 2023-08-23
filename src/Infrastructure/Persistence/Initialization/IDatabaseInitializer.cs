using Squares.Infrastructure.Multitenancy;

namespace Squares.Infrastructure.Persistence.Initialization;
internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(AppTenant tenant, CancellationToken cancellationToken);
}