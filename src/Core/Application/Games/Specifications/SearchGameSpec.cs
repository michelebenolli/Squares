using Squares.Application.Games.Requests;

namespace Squares.Application.Games.Specifications;
public class SearchGameSpec : SearchSpec<Game>
{
    public SearchGameSpec(SearchGameRequest request)
        : base(request)
    {
        Query.Include(x => x.User)
            .OrderByDescending(x => x.Id, !request.HasOrderBy())
            .AsNoTracking();
    }
}
