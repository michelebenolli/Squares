using Squares.Application.Identity.Roles;
using Squares.Application.Identity.Roles.Requests;

namespace Squares.Host.Controllers.Identity;

public class RolesController : VersionNeutralApiController
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("search")]
    [Permission(AppAction.Search, AppResource.Roles)]
    [OpenApiOperation("Search roles using available filters", "")]
    public Task<PagedResponse<RoleDto>> SearchAsync(SearchRoleRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet]
    [Permission(AppAction.View, AppResource.Roles)]
    [OpenApiOperation("Get a list of all roles", "")]
    public Task<List<RoleDto>> GetListAsync(CancellationToken token)
    {
        return _roleService.ListAsync(token);
    }

    [HttpGet("{id}")]
    [Permission(AppAction.View, AppResource.Roles)]
    [OpenApiOperation("Get role details", "")]
    public Task<RoleDto> GetByIdAsync(int id)
    {
        return _roleService.GetByIdAsync(id);
    }

    [HttpPost]
    [Permission(AppAction.Create, AppResource.Roles)]
    [OpenApiOperation("Create a new role", "")]
    public Task<int> CreateAsync(CreateRoleRequest request)
    {
        return _roleService.CreateAsync(request);
    }

    [HttpPut("{id}")]
    [Permission(AppAction.Update, AppResource.Roles)]
    [OpenApiOperation("Update a role", "")]
    public async Task<ActionResult> UpdateAsync(UpdateRoleRequest request, int id)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        await _roleService.UpdateAsync(request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Permission(AppAction.Delete, AppResource.Roles)]
    [OpenApiOperation("Delete a role", "")]
    public Task DeleteAsync(int id)
    {
        return _roleService.DeleteAsync(id);
    }

    [HttpGet("permissions")]
    [Permission(AppAction.View, AppResource.RoleClaims)]
    [OpenApiOperation("Get all the permissions", "")]
    public List<string> GetPermissions()
    {
        return _roleService.GetPermissions();
    }

    [HttpGet("{id}/permissions")]
    [Permission(AppAction.View, AppResource.RoleClaims)]
    [OpenApiOperation("Get role permissions", "")]
    public Task<List<string>> GetPermissionsAsync(int id)
    {
        return _roleService.GetPermissionsAsync(id);
    }

    [HttpPut("{id}/permissions")]
    [Permission(AppAction.Update, AppResource.RoleClaims)]
    [OpenApiOperation("Update the role permissions", "")]
    public async Task<ActionResult> UpdatePermissionsAsync(UpdatePermissionsRequest request, int id, CancellationToken token)
    {
        if (id != request.RoleId)
        {
            return BadRequest();
        }

        await _roleService.UpdatePermissionsAsync(request, token);
        return Ok();
    }
}