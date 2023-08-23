namespace Squares.Application.Common.Models;
public class BaseRequest
{
    public List<Filter>? Filters { get; set; }
    public List<string>? OrderBy { get; set; }
}

public static class BaseRequestExtensions
{
    public static bool HasOrderBy(this BaseRequest request)
    {
        return request.OrderBy?.Any() is true;
    }
}