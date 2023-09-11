using Squares.Application.Games;
using Squares.Application.Games.Requests;

namespace Squares.Host.Controllers.Games;

public class GamesController : VersionedApiController
{
    [HttpPost("search")]
    [Permission(AppAction.Search, AppResource.Games)]
    [OpenApiOperation("Search games using available filters", "")]
    public Task<PagedResponse<GameDto>> SearchAsync(SearchGameRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost]
    [Permission(AppAction.Create, AppResource.Games)]
    [OpenApiOperation("Create a new game", "")]
    public Task<int> CreateAsync(CreateGameRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpDelete("{id:int}")]
    [Permission(AppAction.Delete, AppResource.Games)]
    [OpenApiOperation("Delete a game", "")]
    public Task DeleteAsync(int id)
    {
        return Mediator.Send(new DeleteGameRequest(id));
    }
}