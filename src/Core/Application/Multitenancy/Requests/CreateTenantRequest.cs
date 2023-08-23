namespace Squares.Application.Multitenancy.Requests;

public class CreateTenantRequest : IRequest<string>
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Domain { get; set; } = default!;
    public string? ConnectionString { get; set; }
    public string AdminEmail { get; set; } = default!;
    public DateTime? EndDate { get; set; }
    public string? Issuer { get; set; }
}

public class CreateTenantHandler : IRequestHandler<CreateTenantRequest, string>
{
    private readonly ITenantService _tenantService;

    public CreateTenantHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public Task<string> Handle(CreateTenantRequest request, CancellationToken token)
    {
        return _tenantService.CreateAsync(request, token);
    }
}