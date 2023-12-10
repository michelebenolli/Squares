using SpecificationExtensions = Squares.Application.Common.Specification.SpecificationBuilderExtensions;

namespace Squares.Application.Common.Models;
public class BaseRequest
{
    public List<Filter>? Filters { get; set; }
    public List<string>? OrderBy { get; set; }
    public List<string>? Include { get; set; }
}

public static class BaseRequestExtensions
{
    public static bool HasOrderBy(this BaseRequest request)
    {
        return request.OrderBy?.Any() is true;
    }

    public static T? GetFilter<T>(this BaseRequest request, string name)
    {
        var field = request.Filters?.FirstOrDefault(x => x.Field == name);
        return field?.Value is null ? default :
            (T?)SpecificationExtensions.GetValue(field.Value, typeof(T));
    }

    public static bool Includes(this BaseRequest request, string name)
    {
        return request.Include?.Any(x => x == name) is true;
    }
}
