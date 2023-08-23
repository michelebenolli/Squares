namespace Squares.Application.Identity.Users.Requests;
public class DeleteUserRequest : IRequest
{
    public int Id { get; set; }
    public DeleteUserRequest(int id)
    {
        Id = id;
    }
}

public class DeleteUserHandler : IRequestHandler<DeleteUserRequest>
{
    private readonly IUserService _userService;

    public DeleteUserHandler(
        IUserService userService)
    {
        _userService = userService;
    }

    public async Task Handle(DeleteUserRequest request, CancellationToken _)
    {
        await _userService.DeleteAsync(request.Id);
    }
}