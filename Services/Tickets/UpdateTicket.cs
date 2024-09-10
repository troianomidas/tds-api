using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities.Tickets;
using WebApi.Persistence;

namespace WebApi.Services.Tickets;

public class UpdateTicketRequest : IRequest<Ticket>
{
    public int Id { get; set; }
    public int Status { get; set; }
    public bool HasStoreAnswer { get; set; }
    public bool HasAdminAnswer { get; set; }
}

public class UpdateTicketRequestHandler : IRequestHandler<UpdateTicketRequest, Ticket>
{
    private readonly AppDbContext _context;
    
    public UpdateTicketRequestHandler(AppDbContext context) => _context = context;

    public async Task<Ticket> Handle(UpdateTicketRequest request, CancellationToken cancellationToken)
    {
        var ticketDb = await _context.Tickets.Where(x => x.Id == request.Id)
            .Include(x=>x.Store).ThenInclude(x => x!.User)
            .FirstOrDefaultAsync(cancellationToken);

        if (ticketDb == null)
            throw new InvalidOperationException("Ticket não encontrado.");
        
        ticketDb.UpdatedAt = DateTime.Now;
        ticketDb.Status = request.Status;
        ticketDb.HasAdminAnswer = request.HasAdminAnswer;
        ticketDb.HasStoreAnswer = request.HasStoreAnswer;

        await _context.SaveChangesAsync(cancellationToken);
        
        return ticketDb;
    }
}