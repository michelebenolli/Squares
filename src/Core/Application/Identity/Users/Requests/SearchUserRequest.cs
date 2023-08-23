namespace Squares.Application.Identity.Users.Requests;
public class SearchUserRequest : PagedRequest, IRequest<PagedResponse<ApplicationUserDto>>
{
    public int? RoleId { get; set; }
    public string? FullName { get; set; }
    public bool? IsActive { get; set; }
}

public class SearchUserHandler : IRequestHandler<SearchUserRequest, PagedResponse<ApplicationUserDto>>
{
    private readonly IUserService _userService;

    public SearchUserHandler(
        IUserService userService)
    {
        _userService = userService;
    }

    public async Task<PagedResponse<ApplicationUserDto>> Handle(SearchUserRequest request, CancellationToken token)
    {
        var model = await _userService.SearchAsync(request, token);
        return model.ToPagedResponse();
    }
}