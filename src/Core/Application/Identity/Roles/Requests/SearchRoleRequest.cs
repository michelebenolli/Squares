namespace Squares.Application.Identity.Roles.Requests;
public class SearchRoleRequest : PagedRequest, IRequest<PagedResponse<RoleDto>>
{
}

public class SearchRoleHandler : IRequestHandler<SearchRoleRequest, PagedResponse<RoleDto>>
{
    private readonly IRoleService _roleService;

    public SearchRoleHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<PagedResponse<RoleDto>> Handle(SearchRoleRequest request, CancellationToken token)
    {
        var model = await _roleService.SearchAsync(request, token);
        return model.ToPagedResponse();
    }
}