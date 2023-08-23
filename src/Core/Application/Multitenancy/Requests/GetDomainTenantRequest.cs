namespace Squares.Application.Multitenancy.Requests;

public class GetDomainTenantRequest : IRequest<string>
{
    public string Domain { get; set; } = default!;
    public GetDomainTenantRequest(string domain)
    {
        Domain = domain;
    }
}

public class GetDomainTenantHandler : IRequestHandler<GetDomainTenantRequest, string>
{
    private readonly ITenantService _tenantService;

    public GetDomainTenantHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public Task<string> Handle(GetDomainTenantRequest request, CancellationToken _)
    {
        return _tenantService.GetByDomainAsync(request.Domain);
    }
}