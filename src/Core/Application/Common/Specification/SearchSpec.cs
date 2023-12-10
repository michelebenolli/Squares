using System.Linq.Expressions;

namespace Squares.Application.Common.Specification;
public class SearchSpec<T> : Specification<T>
{
    public SearchSpec(
        BaseRequest request,
        Dictionary<string, Expression<Func<T, object?>>>? orderBy = null)
    {
        var filter = new Filter
        {
            Logic = FilterLogic.AND,
            Filters = request.Filters?.Where(x => x.Field?.StartsWith("@") != true)
        };

        Query.SearchBy(filter).OrderBy(request.OrderBy, orderBy);
    }
}
