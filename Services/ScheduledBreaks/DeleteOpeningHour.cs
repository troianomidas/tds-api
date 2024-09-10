using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Stores;
using WebApi.Domain.Exceptions;

namespace WebApi.Services.ScheduledBreaks;

public record DeleteScheduledBreak : IRequest<bool>
{
    public int StoreId { get; set; }
    public int ScheduledBreakId { get; set; }
}

public class DeleteScheduledBreakHandler : IRequestHandler<DeleteScheduledBreak, bool>
{
    private readonly AppDbContext _context;

    public DeleteScheduledBreakHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteScheduledBreak request, CancellationToken cancellationToken)
    {
        await _context.ScheduledBreaks.Where(x => x.Id == request.ScheduledBreakId && x.StoreId == request.StoreId)
            .ExecuteDeleteAsync(cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}