using Squares.Domain.Games;

namespace Squares.Application.Games.Requests;
public class DeleteGameRequest : IRequest
{
    public int Id { get; set; }
    public DeleteGameRequest(int id)
    {
        Id = id;
    }
}

public class DeleteGameHandler : IRequestHandler<DeleteGameRequest>
{
    private readonly IRepositoryWithEvents<Game> _games;
    private readonly IStringLocalizer<DeleteGameHandler> _localizer;

    public DeleteGameHandler(
        IRepositoryWithEvents<Game> games,
        IStringLocalizer<DeleteGameHandler> localizer)
    {
        _games = games;
        _localizer = localizer;
    }

    public async Task Handle(DeleteGameRequest request, CancellationToken token)
    {
        var model = await _games.GetByIdAsync(request.Id, token);
        _ = model ?? throw new NotFoundException(_localizer["Elemento non trovato"]);

        _games.Delete(model);
        await _games.SaveAsync(token);
    }
}