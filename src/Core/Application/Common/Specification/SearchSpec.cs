namespace Squares.Application.Common.Specification;

public class SearchSpec<T, TResult> : Specification<T, TResult>
{
    public SearchSpec(BaseRequest request)
    {
        Filter reqFilter = new Filter { Logic = "and", Filters = request.Filters };
        Query.SearchBy(reqFilter);
    }
}

public class SearchSpec<T> : Specification<T>
{
    public SearchSpec(BaseRequest request)
    {
        Filter reqFilter = new Filter { Logic = "and", Filters = request.Filters };
        Query.SearchBy(reqFilter).OrderBy(request.OrderBy);
    }
}