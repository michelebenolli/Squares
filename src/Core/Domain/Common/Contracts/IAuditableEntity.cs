namespace Squares.Domain.Common.Contracts;

public interface IAuditableEntity
{
    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; }
    public int ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}