namespace Squares.Application.Auditing.Requests;

public class SearchAuditRequest : PagedRequest, IRequest<PagedResponse<TrailDto>>
{
    public int UserId { get; set; }
}

public class SearchAuditHandler : IRequestHandler<SearchAuditRequest, PagedResponse<TrailDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IAuditService _auditService;

    public SearchAuditHandler(
        ICurrentUser currentUser,
        IAuditService auditService)
    {
        _currentUser = currentUser;
        _auditService = auditService;
    }

    public async Task<PagedResponse<TrailDto>> Handle(SearchAuditRequest request, CancellationToken token)
    {
        request.UserId = _currentUser.GetUserId();
        var model = await _auditService.SearchAsync(request, token);
        return model.ToPagedResponse();
    }
}