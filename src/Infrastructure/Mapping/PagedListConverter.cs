using AutoMapper;
using X.PagedList;

namespace Squares.Infrastructure.Mapping;

/// <summary>
/// Takes a pagedList and convert it in another pagedList if a valid mapping exists.
/// </summary>
/// <typeparam name="T1">Source type.</typeparam>
/// <typeparam name="T2">Destination type.</typeparam>
public class PagedListConverter<T1, T2> : ITypeConverter<IPagedList<T1>, IPagedList<T2>>
    where T1 : class
    where T2 : class
{
    public IPagedList<T2> Convert(IPagedList<T1> source, IPagedList<T2> destination, ResolutionContext context)
    {
        var result = source.Select(context.Mapper.Map<T1, T2>).ToList();
        return new StaticPagedList<T2>(result, source.PageNumber, source.PageSize, source.TotalItemCount);
    }
}