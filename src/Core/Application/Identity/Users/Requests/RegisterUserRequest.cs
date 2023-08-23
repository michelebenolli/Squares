namespace Squares.Application.Identity.Users.Requests;

public class RegisterUserRequest : IRequest<int>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Origin { get; set; }
}

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, int>
{
    private readonly IUserService _userService;

    public RegisterUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<int> Handle(RegisterUserRequest request, CancellationToken _)
    {
        return await _userService.RegisterAsync(request, request.Origin!);
    }
}