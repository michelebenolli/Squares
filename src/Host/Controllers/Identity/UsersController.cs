using Squares.Application.Identity.Roles;
using Squares.Application.Identity.Users;
using Squares.Application.Identity.Users.Requests;

namespace Squares.Host.Controllers.Identity;

public class UsersController : VersionNeutralApiController
{
    [HttpPost("search")]
    [Permission(AppAction.Search, AppResource.Users)]
    [OpenApiOperation("Search users using available filters", "")]
    public Task<PagedResponse<ApplicationUserDto>> SearchAsync(SearchUserRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet]
    [Permission(AppAction.View, AppResource.Users)]
    [OpenApiOperation("Get list of all users", "")]
    public Task<List<ApplicationUserDto>> GetListAsync()
    {
        return Mediator.Send(new GetUsersRequest());
    }

    [HttpGet("{id}")]
    [Permission(AppAction.View, AppResource.Users)]
    [OpenApiOperation("Get user details", "")]
    public Task<ApplicationUserDto> GetAsync(int id)
    {
        return Mediator.Send(new GetUserRequest(id));
    }

    [HttpPost]
    [Permission(AppAction.Create, AppResource.Users)]
    [OpenApiOperation("Create a new user", "")]
    public Task<int> CreateAsync(CreateUserRequest request)
    {
        request.Origin = GetOrigin();
        return Mediator.Send(request);
    }

    [HttpPost("register")]
    [TenantIdHeader]
    [AllowAnonymous]
    [OpenApiOperation("Anonymous user creates a user", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public Task<int> RegisterAsync(RegisterUserRequest request)
    {
        request.Origin = GetOrigin();
        return Mediator.Send(request);
    }

    [HttpPut("{id}")]
    [Permission(AppAction.Update, AppResource.Users)]
    [OpenApiOperation("Update a user", "")]
    public async Task<ActionResult> UpdateAsync(UpdateUserRequest request, int id)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(request);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Permission(AppAction.Delete, AppResource.Users)]
    [OpenApiOperation("Delete a user", "")]
    public Task DeleteAsync(int id)
    {
        return Mediator.Send(new DeleteUserRequest(id));
    }

    [HttpGet("{id}/roles")]
    [Permission(AppAction.View, AppResource.UserRoles)]
    [OpenApiOperation("Get the list of user roles", "")]
    public Task<List<RoleDto>> GetRolesAsync(int id)
    {
        return Mediator.Send(new GetRolesRequest(id));
    }

    [HttpPut("{id}/toggle")]
    [Permission(AppAction.Update, AppResource.Users)]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    [OpenApiOperation("Toggle the user status", "")]
    public async Task<ActionResult> ToggleAsync(int id, ToggleUserRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(request);
        return Ok();
    }

    private string GetOrigin() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
}