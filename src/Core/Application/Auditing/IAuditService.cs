using Squares.Application.Auditing.Requests;

namespace Squares.Application.Auditing;

public interface IAuditService : ITransientService
{
    Task<IPagedList<TrailDto>> SearchAsync(SearchAuditRequest request, CancellationToken token);
}