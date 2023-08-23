using Squares.Application.Identity.Roles;

namespace Squares.Application.Identity.Users.Requests;
public class GetRolesRequest : IRequest<List<RoleDto>>
{
    public int Id { get; set; }
    public GetRolesRequest(int id)
    {
        Id = id;
    }
}

public class GetRoleHandler : IRequestHandler<GetRolesRequest, List<RoleDto>>
{
    private readonly IUserService _userService;

    public GetRoleHandler(
        IUserService userService)
    {
        _userService = userService;
    }

    public async Task<List<RoleDto>> Handle(GetRolesRequest request, CancellationToken token)
    {
        return await _userService.GetRolesAsync(request.Id, token);
    }
}