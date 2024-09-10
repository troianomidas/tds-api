using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities.Tickets;
using WebApi.Persistence;

namespace WebApi.Services.Tickets;

public class GetTicketByIdRequest : IRequest<Ticket?>
{
    public int Id { get; set; }
    public int StoreId { get; set; }
}

public class GetTicketByIdRequestHandler : IRequestHandler<GetTicketByIdRequest, Ticket?>
{
    private readonly AppDbContext _context;

    public GetTicketByIdRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Ticket?> Handle(GetTicketByIdRequest request, CancellationToken cancellationToken)
    {
        if (request.Id < 0)
            throw new InvalidOperationException("TicketId is required.");

        if (request.StoreId < 0)
            throw new InvalidOperationException("StoreId is required.");

        return await _context.Tickets.Where(x => x.Id == request.Id && x.StoreId == request.StoreId)
            .Include(x => x.TicketAnswer)
            .Include(x => x.Store)
            .FirstOrDefaultAsync(cancellationToken);
    }
}