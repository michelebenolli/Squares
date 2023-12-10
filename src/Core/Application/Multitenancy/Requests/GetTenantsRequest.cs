namespace Squares.Application.Multitenancy.Requests;
public class GetTenantsRequest : BaseRequest, IRequest<List<TenantDto>>
{
}

public class GetTenantsHandler : IRequestHandler<GetTenantsRequest, List<TenantDto>>
{
    private readonly ITenantService _tenantService;

    public GetTenantsHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public Task<List<TenantDto>> Handle(GetTenantsRequest request, CancellationToken _)
    {
        return _tenantService.ListAsync();
    }
}
