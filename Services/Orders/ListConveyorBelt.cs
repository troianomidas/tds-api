using WebApi.Domain.Constants;
using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Orders;

public record ListConveyorBeltRequest : IRequest<List<Order>>
{
    public int StoreId { get; init; }
}

public class ListConveyorBeltRequestHandler : IRequestHandler<ListConveyorBeltRequest, List<Order>>
{
    private readonly AppDbContext _context;

    public ListConveyorBeltRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> Handle(ListConveyorBeltRequest request,
        CancellationToken cancellationToken)
    {
        if (request.StoreId < 1)
            throw new InvalidOperationException("StoreId is required.");

        return await _context.Orders.Where(x => x.StoreId == request.StoreId && new List<int>{OrderStatusConst.Pending,OrderStatusConst.Prepare,OrderStatusConst.Delivery}.Contains(x.Status))
            .Include(x=> x.ShippingAddress)
            .Include(x=> x.Collaborator)
            .Include(x=>x.Items)
            .ToListAsync(cancellationToken);
    }
}