using WebApi.Domain.Common;
using WebApi.Domain.Messages;

namespace WebApi.Domain.Entities.Tickets.TicketAnswers;

public class TicketAnswer : BaseEntity
{
    public TicketAnswer()
    {
        
    }

    public TicketAnswer(int ticketId, string? body)
    {
        if (ticketId < 0)
            throw new InvalidOperationException(InputMsg.Required);
        
        if (string.IsNullOrEmpty(body))
            throw new InvalidOperationException(InputMsg.BodyRequired);

        TicketId = ticketId;
        Body = body;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        IsStoreAnswer = false;
    }
    
    public int TicketId { get; set; }
    public string? Body { get; set; }
    public bool IsStoreAnswer { get; set; }
    public int Sort { get; set; }
    public DateTime UpdatedAt { get; set; }
}
