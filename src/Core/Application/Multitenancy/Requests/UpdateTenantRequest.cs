namespace Squares.Application.Multitenancy.Requests;

public class UpdateTenantRequest : IRequest<Unit>
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Domain { get; set; } = default!;
    public string? ConnectionString { get; set; }
    public string AdminEmail { get; set; } = default!;
    public DateTime? EndDate { get; set; }
    public string? Issuer { get; set; }
}

public class UpgradeTenantHandler : IRequestHandler<UpdateTenantRequest, Unit>
{
    private readonly ITenantService _tenantService;

    public UpgradeTenantHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public async Task<Unit> Handle(UpdateTenantRequest request, CancellationToken _)
    {
        await _tenantService.UpdateAsync(request);
        return Unit.Value;
    }
}