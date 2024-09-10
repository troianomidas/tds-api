using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;

namespace WebApi.Services.ScheduledBreaks;

public class GetScheduledBreak : IRequest<List<ScheduledBreak>>
{
    public int StoreId { get; set; }
}

public class GetScheduledBreakRequestHandler : IRequestHandler<GetScheduledBreak, List<ScheduledBreak>>
{
    private readonly AppDbContext _context;

    public GetScheduledBreakRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ScheduledBreak>> Handle(GetScheduledBreak request, CancellationToken cancellationToken)
    {
        return await _context.ScheduledBreaks.Where(x => x.StoreId == request.StoreId).ToListAsync(cancellationToken);
    }
}