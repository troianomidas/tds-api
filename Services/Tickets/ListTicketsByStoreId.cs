using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities.Tickets;
using WebApi.Persistence;

namespace WebApi.Services.Tickets;

public class ListTicketsByStoreIdRequest : IRequest<List<Ticket>?>
{
    public int StoreId { get; set; }
}

public class ListTicketsByStoreIdRequestHandler : IRequestHandler<ListTicketsByStoreIdRequest, List<Ticket>?>
{
    private readonly AppDbContext _context;

    public ListTicketsByStoreIdRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ticket>?> Handle(ListTicketsByStoreIdRequest request, CancellationToken cancellationToken)
    {
        if (request.StoreId < 0)
            throw new InvalidOperationException("StoreId is required.");

        return await _context.Tickets.Where(x => x.StoreId == request.StoreId)
            .Include(x => x.TicketAnswer)
            .Include(x => x.Store)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}