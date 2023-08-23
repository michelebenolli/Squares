namespace Squares.Domain.Common.Contracts;
public abstract class AuditableEntity : BaseEntity, IAuditableEntity
{
    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; private set; }
    public int ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }

    protected AuditableEntity()
    {
        CreatedOn = DateTime.UtcNow;
        ModifiedOn = DateTime.UtcNow;
    }
}