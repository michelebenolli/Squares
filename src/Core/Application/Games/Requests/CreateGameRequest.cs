using Squares.Domain.Games;

namespace Squares.Application.Games.Requests;
public class CreateGameRequest : IRequest<int>
{
    public int Score { get; set; } = default!;
}

public class CreateGameHandler : IRequestHandler<CreateGameRequest, int>
{
    private readonly IRepositoryWithEvents<Game> _games;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateGameHandler(
        IRepositoryWithEvents<Game> games,
        IMapper mapper,
        ICurrentUser currentUser)
    {
        _games = games;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(CreateGameRequest request, CancellationToken token)
    {
        var model = _mapper.Map<Game>(request);
        model.UserId = _currentUser.GetUserId();
        model.DateTime = DateTime.UtcNow;

        _games.Insert(model);
        await _games.SaveAsync(token);

        return model.Id;
    }
}