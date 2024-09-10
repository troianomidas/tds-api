using WebApi.Domain.Common;
using WebApi.Domain.Entities.Stores;
using WebApi.Domain.Entities.Tickets.TicketAnswers;
using WebApi.Domain.Messages;

namespace WebApi.Domain.Entities.Tickets;

public class Ticket : BaseStoreEntity
{
    public Ticket() => TicketAnswer = new List<TicketAnswer>();

    public Ticket(int storeId, string? title, string? body)
    {
        if (storeId < 0)
            throw new InvalidOperationException(InputMsg.Required);

        if (string.IsNullOrEmpty(title))
            throw new InvalidOperationException(InputMsg.TitleRequired);

        if (title.Length is < 4 or > 65)
            throw new InvalidOperationException(InputMsg.TitleLengthMin4Max65);
        
        if (string.IsNullOrEmpty(body))
            throw new InvalidOperationException(InputMsg.BodyRequired);

        StoreId = storeId;
        Title = title;
        Body = body;
        Status = 1;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        HasStoreAnswer = true;
        HasAdminAnswer = false;
    }
    
    public string? Title { get; set; }
    public string? Body { get; set; }
    public int Status { get; set; }
    public bool HasStoreAnswer { get; set; }
    public bool HasAdminAnswer { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<TicketAnswer>? TicketAnswer { get; set; }
    public virtual Store? Store { get; set; }
}