namespace Squares.Application.Common.Models;

public class PagedResponse<T>
{
    public List<T> Data { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public PagedResponse(List<T> data, IPagedList metadata)
    {
        Data = data;
        CurrentPage = metadata.PageNumber;
        PageSize = metadata.PageSize;
        TotalPages = metadata.PageCount;
        TotalCount = metadata.TotalItemCount;
    }
}