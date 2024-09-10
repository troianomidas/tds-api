using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Stores;

public record GetStoreDeliveryRequest : IRequest<StoreDelivery?>
{
    public int StoreId { get; set; }
}

public class GetStoreDeliveryRequestHandler : IRequestHandler<GetStoreDeliveryRequest, StoreDelivery?>
{
    private readonly AppDbContext _context;

    public GetStoreDeliveryRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StoreDelivery?> Handle(GetStoreDeliveryRequest request, CancellationToken cancellationToken)
    {
        return await _context.StoreDeliveries.Where(x => x.StoreId == request.StoreId).FirstOrDefaultAsync(cancellationToken);
    }
}