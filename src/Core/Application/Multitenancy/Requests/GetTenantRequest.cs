namespace Squares.Application.Multitenancy.Requests;

public class GetTenantRequest : IRequest<TenantDto>
{
    public string TenantId { get; set; } = default!;
    public GetTenantRequest(string tenantId)
    {
        TenantId = tenantId;
    }
}

public class GetTenantHandler : IRequestHandler<GetTenantRequest, TenantDto>
{
    private readonly ITenantService _tenantService;

    public GetTenantHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public Task<TenantDto> Handle(GetTenantRequest request, CancellationToken _)
    {
        return _tenantService.GetByIdAsync(request.TenantId);
    }
}