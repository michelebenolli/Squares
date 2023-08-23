namespace Squares.Application.Identity.Users.Requests;
public class GetUsersRequest : IRequest<List<ApplicationUserDto>>
{
}

public class GetUsersHandler : IRequestHandler<GetUsersRequest, List<ApplicationUserDto>>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetUsersHandler(
        IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<List<ApplicationUserDto>> Handle(GetUsersRequest request, CancellationToken token)
    {
        var model = await _userService.ListAsync(token);
        return _mapper.Map<List<ApplicationUserDto>>(model);
    }
}