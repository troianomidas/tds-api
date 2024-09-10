using MediatR;
using WebApi.Domain.Entities.Tickets;
using WebApi.Persistence;

namespace WebApi.Services.Tickets;

public class CreateTicketRequest : IRequest<Ticket>
{
    public int StoreId { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
}

public class CreateTicketRequestHandler : IRequestHandler<CreateTicketRequest, Ticket>
{
    private readonly AppDbContext _context;
    public CreateTicketRequestHandler(AppDbContext context) => _context = context;

    public async Task<Ticket> Handle(CreateTicketRequest request, CancellationToken cancellationToken)
    {
        Ticket newTicket = new Ticket(request.StoreId, request.Title, request.Body);

        _context.Tickets.Add(newTicket);

        await _context.SaveChangesAsync(cancellationToken);

        return newTicket;
    }
}