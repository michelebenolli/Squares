namespace Squares.Application.Multitenancy;

public class TenantDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string AdminEmail { get; set; } = default!;
    public string? Domain { get; set; }
    public string? ConnectionString { get; set; }
    public bool IsActive { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Issuer { get; set; }
}