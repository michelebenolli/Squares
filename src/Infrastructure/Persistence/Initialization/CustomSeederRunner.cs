using Microsoft.Extensions.DependencyInjection;

namespace Squares.Infrastructure.Persistence.Initialization;
internal class CustomSeederRunner
{
    private readonly ISeeder[] _seeders;

    public CustomSeederRunner(IServiceProvider serviceProvider)
    {
        _seeders = serviceProvider.GetServices<ISeeder>()
            .OrderBy(x => x.Order).ToArray();
    }

    public async Task RunSeedersAsync(CancellationToken token)
    {
        foreach (var seeder in _seeders)
        {
            await seeder.SeedAsync(token);
        }
    }
}