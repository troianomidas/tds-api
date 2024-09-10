using WebApi.Domain.Constants;
using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Orders;

public class UpdateOrderStatusRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public List<int> OrderIds { get; set; } = new();
    public int? CollaboratorId { get; set; }
    public int Status { get; set; }
}

public class UpdateOrderStatusRequestHandler : IRequestHandler<UpdateOrderStatusRequest, bool>
{
    private readonly AppDbContext _context;

    public UpdateOrderStatusRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
    {
        List<Order> orders = await _context.Orders.Where(x => x.StoreId == request.StoreId && request.OrderIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
        
        orders.ForEach(x =>
        {
            x.Status = request.Status;
            x.CollaboratorId = request.CollaboratorId ?? x.CollaboratorId;
        });
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}