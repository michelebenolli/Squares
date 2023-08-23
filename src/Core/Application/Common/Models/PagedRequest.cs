namespace Squares.Application.Common.Models;
public class PagedRequest : BaseRequest
{
    private int _pageNumber;
    private int _pageSize;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value <= 0 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value <= 0 ? 10 : value > 100 ? 100 : value;
    }
}