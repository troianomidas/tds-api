using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Orders;

public record GetOrderRequest : IRequest<Order?>
{
    public int StoreId { get; set; }
    public int OrderId { get; set; }
    public long TrackId { get; set; }
}

public class GetOrderRequestHandler : IRequestHandler<GetOrderRequest, Order?>
{
    private readonly AppDbContext _context;

    public GetOrderRequestHandler(AppDbContext context) => _context = context;

    public async Task<Order?> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {
        if (request.TrackId > 0)
        {
            return await _context.Orders.Where(x=> x.TrackId == request.TrackId)
                .Include(x => x.PaymentMethod)
                .Include(x => x.ShippingAddress)
                .Include(x => x.Items)
                .Include(x => x.Collaborator)
                .Include(x => x.Store)
                .ThenInclude(x=> x.Address)
                .FirstOrDefaultAsync(cancellationToken);
        }
        
        if (request.StoreId < 1)
            throw new InvalidOperationException("StoreId is required.");
        
        if (request.OrderId < 1)
            throw new InvalidOperationException("OrderId is required.");

        return await _context.Orders.Where(x => x.StoreId == request.StoreId && x.Id == request.OrderId)
            .Include(x => x.PaymentMethod)
            .Include(x => x.Items)
            .Include(x => x.Collaborator)
            .FirstOrDefaultAsync(cancellationToken);
    }
}