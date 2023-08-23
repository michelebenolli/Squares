namespace Squares.Application.Common.Models;

public static class PagedListExtensions
{
    public static PagedResponse<T> ToPagedResponse<T>(this IPagedList<T> model)
    {
        return new PagedResponse<T>(model.ToList(), model);
    }
}