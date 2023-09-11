using Squares.Domain.Games;

namespace Squares.Application.Games.Requests;
public class SearchGameRequest : PagedRequest, IRequest<PagedResponse<GameDto>>
{
}

public class SearchGameHandler : IRequestHandler<SearchGameRequest, PagedResponse<GameDto>>
{
    private readonly IReadRepository<Game> _games;
    private readonly IMapper _mapper;

    public SearchGameHandler(
        IReadRepository<Game> games,
        IMapper mapper)
    {
        _games = games;
        _mapper = mapper;
    }

    public async Task<PagedResponse<GameDto>> Handle(SearchGameRequest request, CancellationToken token)
    {
        var model = await _games.PagedListAsync(new SearchSpec<Game>(request), request.PageNumber, request.PageSize, token);
        return _mapper.Map<IPagedList<GameDto>>(model).ToPagedResponse();
    }
}