namespace Squares.Application.Multitenancy.Requests;

public class ToggleTenantRequest : IRequest<Unit>
{
    public string TenantId { get; set; } = default!;
    public ToggleTenantRequest(string tenantId)
    {
        TenantId = tenantId;
    }
}

public class ToggleTenantHandler : IRequestHandler<ToggleTenantRequest, Unit>
{
    private readonly ITenantService _tenantService;

    public ToggleTenantHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public async Task<Unit> Handle(ToggleTenantRequest request, CancellationToken _)
    {
        await _tenantService.ToggleAsync(request.TenantId);
        return Unit.Value;
    }
}