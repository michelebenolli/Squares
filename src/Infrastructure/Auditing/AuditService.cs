using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using Squares.Application.Auditing;
using Squares.Application.Auditing.Requests;
using Squares.Infrastructure.Persistence.Context;
using X.PagedList;

namespace Squares.Infrastructure.Auditing;

public class AuditService : IAuditService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public AuditService(
        DatabaseContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IPagedList<TrailDto>> SearchAsync(SearchAuditRequest request, CancellationToken token)
    {
        var model = await _context.AuditTrails
            .WithSpecification(new SearchAuditSpec(request))
            .ToPagedListAsync(request.PageNumber, request.PageSize, token);

        return _mapper.Map<IPagedList<TrailDto>>(model);
    }
}