using Squares.Application.Multitenancy;
using Squares.Application.Multitenancy.Requests;

namespace Squares.Host.Controllers.Multitenancy;

public class TenantsController : VersionNeutralApiController
{
    [HttpGet]
    [Permission(AppAction.View, AppResource.Tenants)]
    [OpenApiOperation("Get a list of all tenants", "")]
    public Task<List<TenantDto>> GetListAsync()
    {
        return Mediator.Send(new GetTenantsRequest());
    }

    [HttpGet("{id}")]
    [Permission(AppAction.View, AppResource.Tenants)]
    [OpenApiOperation("Get tenant details", "")]
    public Task<TenantDto> GetAsync(string id)
    {
        return Mediator.Send(new GetTenantRequest(id));
    }

    [HttpPost]
    [Permission(AppAction.Create, AppResource.Tenants)]
    [OpenApiOperation("Create a new tenant", "")]
    public Task<string> CreateAsync(CreateTenantRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id}")]
    [Permission(AppAction.Update, AppResource.Tenants)]
    [OpenApiOperation("Update the tenant", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public async Task<ActionResult> UpdateAsync(UpdateTenantRequest request, string id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpPost("{id}/toggle")]
    [Permission(AppAction.Update, AppResource.Tenants)]
    [OpenApiOperation("Enable or disable a tenant", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public async Task<ActionResult> ToggleAsync(string id)
    {
        return Ok(await Mediator.Send(new ToggleTenantRequest(id)));
    }

    [HttpGet("current/{domain}")]
    [AllowAnonymous]
    [OpenApiOperation("Get id of the current tenant", "")]
    public Task<string> GetCurrentAsync(string domain)
    {
        return Mediator.Send(new GetDomainTenantRequest(domain));
    }
}