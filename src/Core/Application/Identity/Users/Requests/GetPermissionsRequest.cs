namespace Squares.Application.Identity.Users.Requests;
public class GetPermissionsRequest : IRequest<List<string>>
{
    public int Id { get; set; }
    public GetPermissionsRequest(int id)
    {
        Id = id;
    }
}

public class GetPermissionsHandler : IRequestHandler<GetPermissionsRequest, List<string>>
{
    private readonly IUserService _userService;

    public GetPermissionsHandler(
        IUserService userService)
    {
        _userService = userService;
    }

    public async Task<List<string>> Handle(GetPermissionsRequest request, CancellationToken token)
    {
        return await _userService.GetPermissionsAsync(request.Id, token);
    }
}