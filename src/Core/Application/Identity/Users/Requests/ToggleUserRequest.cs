namespace Squares.Application.Identity.Users.Requests;
public class ToggleUserRequest : IRequest
{
    public int Id { get; set; } = default!;
    public bool Active { get; set; }
}

public class ToggleUserHandler : IRequestHandler<ToggleUserRequest>
{
    private readonly IUserService _userService;

    public ToggleUserHandler(
        IUserService userService)
    {
        _userService = userService;
    }

    public async Task Handle(ToggleUserRequest request, CancellationToken token)
    {
        await _userService.ToggleAsync(request.Id, request.Active, token);
    }
}