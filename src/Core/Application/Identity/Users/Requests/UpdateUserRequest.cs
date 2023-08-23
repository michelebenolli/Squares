namespace Squares.Application.Identity.Users.Requests;
public class UpdateUserRequest : IRequest
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }

    public List<int>? Roles { get; set; }
}

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest>
{
    private readonly IUserService _userService;

    public UpdateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task Handle(UpdateUserRequest request, CancellationToken token)
    {
        await _userService.UpdateAsync(request, token);
    }
}