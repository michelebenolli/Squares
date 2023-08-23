namespace Squares.Application.Identity.Users.Requests;
public class CreateUserRequest : IRequest<int>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }

    public List<int>? Roles { get; set; }
}

public class CreateUserHandler : IRequestHandler<CreateUserRequest, int>
{
    private readonly IUserService _userService;

    public CreateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<int> Handle(CreateUserRequest request, CancellationToken _)
    {
        return await _userService.CreateAsync(request, string.Empty);
    }
}