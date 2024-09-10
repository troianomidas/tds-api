using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

namespace WebApi.Services.OpeningHours;

public record CreateOrUpdateOpeningHourRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public List<CreateOrUpdateOpeningHourItemRequest>? Items { get; set; }

    public class CreateOrUpdateOpeningHourItemRequest
    {
        public int Id { get; set; }
        public string? DayOfWeek { get; set; }
        public string? BeginAt { get; set; }
        public string? EndAt { get; set; }
        public int ScheduleType { get; set; }
        public int Sort { get; set; }
    }
}

public class CreateOrUpdateOpeningHourRequestHandler : IRequestHandler<CreateOrUpdateOpeningHourRequest, bool>
{
    private readonly AppDbContext _context;

    public CreateOrUpdateOpeningHourRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateOrUpdateOpeningHourRequest request, CancellationToken cancellationToken)
    {
        if (request.Items == null)
            return true;

        Store? storeDb = await _context.Stores.Where(x => x.Id == request.StoreId).Include(x => x.OpeningHours)
            .FirstOrDefaultAsync(cancellationToken);

        if (storeDb == null)
            throw new InvalidOperationException("Loja nao encontrada");

        List<OpeningHour> openingHours = new();
        
        foreach (CreateOrUpdateOpeningHourRequest.CreateOrUpdateOpeningHourItemRequest item in request.Items)
        {
            openingHours.Add(new OpeningHour(request.StoreId, item.DayOfWeek, item.BeginAt, item.EndAt, item.ScheduleType, item.Sort)
            {
                //set id for update instead of create
                Id = item.Id,
            });
        }

        storeDb.OpeningHours = openingHours;
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;    
    }
}