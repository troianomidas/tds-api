using WebApi.Domain.Entities;
using WebApi.Persistence;
using WebApi.Services.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities.Stores;
using WebApi.Services.OpeningHours;

namespace WebApi.Services.Stores;

public record CreateOrUpdateDeliveryAreas : IRequest<bool>
{
    public int StoreId { get; set; }
    public List<CreateOrUpdateDeliveryAreasItem>? Items { get; set; }

    public class CreateOrUpdateDeliveryAreasItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Fee { get; set; }
    }
}

public class CreateOrUpdateDeliveryAreasHandler : IRequestHandler<CreateOrUpdateDeliveryAreas, bool>
{
    private readonly AppDbContext _context;

    public CreateOrUpdateDeliveryAreasHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateOrUpdateDeliveryAreas request, CancellationToken cancellationToken)
    {
        if (request.Items == null)
            return true;

        Store? storeDb = await _context.Stores.Where(x => x.Id == request.StoreId).Include(x => x.DeliveryAreas)
            .FirstOrDefaultAsync(cancellationToken);

        if (storeDb == null)
            throw new InvalidOperationException("Loja nao encontrada");

        List<DeliveryArea> deliveryAreas = new();
        
        foreach (CreateOrUpdateDeliveryAreas.CreateOrUpdateDeliveryAreasItem item in request.Items)
        {
            deliveryAreas.Add(new DeliveryArea(request.StoreId, item.Name, item.Fee)
            {
                //set id for update instead of create
                Id = item.Id,
            });
        }

        storeDb.DeliveryAreas = deliveryAreas;
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}