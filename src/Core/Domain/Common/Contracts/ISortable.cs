namespace Squares.Domain.Common.Contracts;

public interface ISortable
{
    int Id { get; }
    int? Order { get; set; }
}