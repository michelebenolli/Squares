using Squares.Domain.Identity;

namespace Squares.Application.Identity.Users.Requests;
public class GetUsersRequest : BaseRequest, IRequest<List<ApplicationUserDto>>
{
}

public class GetUsersHandler : IRequestHandler<GetUsersRequest, List<ApplicationUserDto>>
{
    private readonly IReadRepository<User> _users;
    private readonly IMapper _mapper;

    public GetUsersHandler(
        IReadRepository<User> users,
        IMapper mapper)
    {
        _users = users;
        _mapper = mapper;
    }

    public async Task<List<ApplicationUserDto>> Handle(GetUsersRequest request, CancellationToken token)
    {
        var model = await _users.ListAsync(new SearchSpec<User>(request), token);
        return _mapper.Map<List<ApplicationUserDto>>(model);
    }
}
