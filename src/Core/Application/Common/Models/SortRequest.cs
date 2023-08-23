namespace Squares.Application.Common.Models;

public class SortRequest : IRequest
{
    public List<int> Ids { get; set; } = default!;
}