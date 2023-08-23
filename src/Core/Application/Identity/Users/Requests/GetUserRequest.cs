namespace Squares.Application.Identity.Users.Requests;
public class GetUserRequest : IRequest<ApplicationUserDto>
{
    public int Id { get; set; }
    public GetUserRequest(int id)
    {
        Id = id;
    }
}

public class GetUserHandler : IRequestHandler<GetUserRequest, ApplicationUserDto>
{
    private readonly IUserService _userService;
    private readonly IStringLocalizer _localizer;
    private readonly IMapper _mapper;

    public GetUserHandler(
        IUserService userService,
        IStringLocalizer<GetUserHandler> localizer,
        IMapper mapper)
    {
        _userService = userService;
        _localizer = localizer;
        _mapper = mapper;
    }

    public async Task<ApplicationUserDto> Handle(GetUserRequest request, CancellationToken token)
    {
        var model = await _userService.GetByIdAsync(request.Id, token);
        _ = model ?? throw new NotFoundException(_localizer["Elemento non trovato"]);

        return _mapper.Map<ApplicationUserDto>(model);
    }
}