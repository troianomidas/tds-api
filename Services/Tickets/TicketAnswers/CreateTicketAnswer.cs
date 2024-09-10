using WebApi.Domain.Entities.Tickets.TicketAnswers;
using WebApi.Persistence;
using WebApi.Services.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Tickets.TicketAnswers;

public class CreateTicketAnswerRequest : IRequest<TicketAnswer>
{
    public int StoreId { get; set; }
    public int TicketId { get; set; }
    public string? Body { get; set; }
}

public class CreateTicketAnswerRequestHandler : IRequestHandler<CreateTicketAnswerRequest, TicketAnswer>
{
    private readonly AppDbContext _context;
    
    public CreateTicketAnswerRequestHandler(AppDbContext context) => _context = context;

    public async Task<TicketAnswer> Handle(CreateTicketAnswerRequest request, CancellationToken cancellationToken)
    {
        if (!_context.Tickets.Any(x => x.Id == request.TicketId))
            throw new InvalidOperationException("Ticket não encontrado.");

        TicketAnswer newTicketAnswer = new TicketAnswer(request.TicketId, request.Body);

        if (request.StoreId != 9999)
            newTicketAnswer.IsStoreAnswer = true;

        if (_context.TicketAnswers.Any(x => x.TicketId == request.TicketId))
            newTicketAnswer.Sort = _context.TicketAnswers.OrderBy(x => x.Sort).Last(x => x.TicketId == request.TicketId).Sort + 1;
        else
            newTicketAnswer.Sort = 1;
        

        _context.TicketAnswers.Add(newTicketAnswer);

        await _context.SaveChangesAsync(cancellationToken);

        return newTicketAnswer;
    }
}