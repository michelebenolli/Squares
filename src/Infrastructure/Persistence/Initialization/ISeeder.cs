namespace Squares.Infrastructure.Persistence.Initialization;

public interface ISeeder
{
    Task SeedAsync(CancellationToken token);
    int? Order { get; }
}