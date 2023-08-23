using Ardalis.Specification;
using Squares.Application.Auditing.Requests;
using Squares.Application.Common.Models;
using Squares.Application.Common.Specification;

namespace Squares.Infrastructure.Auditing;

public class SearchAuditSpec : SearchSpec<Trail>
{
    public SearchAuditSpec(SearchAuditRequest request)
        : base(request)
    {
        Query.Where(x => x.UserId == request.UserId)
            .OrderBy(request.OrderBy)
            .OrderByDescending(x => x.DateTime, !request.HasOrderBy())
            .AsNoTracking();
    }
}