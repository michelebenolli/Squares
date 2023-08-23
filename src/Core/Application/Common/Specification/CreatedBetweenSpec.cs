namespace Squares.Application.Common.Specification;

public class CreatedBetweenSpec<T> : Specification<T>
    where T : IAuditableEntity
{
    public CreatedBetweenSpec(DateTime from, DateTime until)
    {
        Query.Where(e => e.CreatedOn >= from && e.CreatedOn <= until);
    }
}