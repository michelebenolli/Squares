using Finbuckle.MultiTenant;

namespace Squares.Infrastructure.Multitenancy;

public class AppTenant : ITenantInfo
{
    public string Id { get; set; } = default!;
    public string Identifier { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string AdminEmail { get; set; } = default!;
    public string? Domain { get; set; }
    public string? ConnectionString { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? EndDate { get; set; }
    public string? Issuer { get; set; }

    string? ITenantInfo.Id { get => Id; set => Id = value ?? throw new InvalidOperationException("Id can't be null"); }
    string? ITenantInfo.Identifier { get => Identifier; set => Identifier = value ?? throw new InvalidOperationException("Identifier can't be null"); }
    string? ITenantInfo.Name { get => Name; set => Name = value ?? throw new InvalidOperationException("Name can't be null"); }
}