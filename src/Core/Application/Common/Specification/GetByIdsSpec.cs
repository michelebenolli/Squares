namespace Squares.Application.Common.Specification;

public class GetByIdsSpec<T> : Specification<T>
    where T : IEntity
{
    public GetByIdsSpec(List<int> ids)
    {
        Query.Where(x => ids.Contains(x.Id));
    }
}