using WebApi.Domain.Entities.Tickets.TicketAnswers;
using WebApi.Persistence;
using WebApi.Services.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Tickets.TicketAnswers;

public class UpdateTicketAnswerRequest : IRequest<TicketAnswer>
{
    public int Id { get; set; }
    public string? Body { get; set; }
}

public class UpdateTicketAnswerRequestHandler : IRequestHandler<UpdateTicketAnswerRequest, TicketAnswer>
{
    private readonly AppDbContext _context;

    public UpdateTicketAnswerRequestHandler(AppDbContext context) => _context = context;

    public async Task<TicketAnswer> Handle(UpdateTicketAnswerRequest request, CancellationToken cancellationToken)
    {
        var ticketDb = await _context.TicketAnswers.Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (ticketDb == null)
            throw new InvalidOperationException("Resposta de Ticket não encontrada.");
        
        TicketAnswer newTicket = new TicketAnswer(request.Id, request.Body)
        {
            UpdatedAt = DateTimeUtils.Now()
        };
        
        if (!request.Body!.Equals(ticketDb.Body))
            ticketDb.Body = newTicket.Body;
        
        ticketDb.UpdatedAt = newTicket.UpdatedAt;

        await _context.SaveChangesAsync(cancellationToken);

        return ticketDb;
    }
}