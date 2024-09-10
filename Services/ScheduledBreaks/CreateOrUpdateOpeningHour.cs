using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.ScheduledBreaks;

public record CreateOrUpdateScheduledBreak : IRequest<bool>
{
    public int StoreId { get; set; }
    public List<CreateOrUpdateScheduledBreakItem>? Items { get; set; }

    public class CreateOrUpdateScheduledBreakItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }
}

public class CreateOrUpdateScheduledBreakHandler : IRequestHandler<CreateOrUpdateScheduledBreak, bool>
{
    private readonly AppDbContext _context;

    public CreateOrUpdateScheduledBreakHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateOrUpdateScheduledBreak request, CancellationToken cancellationToken)
    {
        if (request.Items == null)
            return true;

        Store? storeDb = await _context.Stores.Where(x => x.Id == request.StoreId).Include(x => x.ScheduledBreaks)
            .FirstOrDefaultAsync(cancellationToken);

        if (storeDb == null)
            throw new InvalidOperationException("Loja nao encontrada");

        List<ScheduledBreak> scheduledBreaks = new();
        
        foreach (CreateOrUpdateScheduledBreak.CreateOrUpdateScheduledBreakItem item in request.Items)
        {
            scheduledBreaks.Add(new ScheduledBreak(request.StoreId, item.Title, item.StartAt, item.EndAt)
            {
                Id = item.Id,
            });
        }

        storeDb.ScheduledBreaks = scheduledBreaks;
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;    
    }
}