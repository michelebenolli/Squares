namespace Squares.Domain.Common.Contracts;

public interface ISoftDelete
{
    DateTime? DeletedOn { get; set; }
    int? DeletedBy { get; set; }
}