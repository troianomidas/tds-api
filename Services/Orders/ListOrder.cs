using AutoMapper;
using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Orders;

public record ListOrderRequest : IRequest<List<Order>>
{
    public int StoreId { get; set; }
    public int Status { get; set; }
    public int Number { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class ListOrderRequestHandler : IRequestHandler<ListOrderRequest, List<Order>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ListOrderRequestHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Order>> Handle(
        ListOrderRequest request,
        CancellationToken cancellationToken)
    {
        if (request.StoreId < 1)
            throw new InvalidOperationException("StoreId is required.");

        IQueryable<Order> query = _context.Orders.Where(x => x.StoreId == request.StoreId && x.CreatedAt.Date >= request.From && x.CreatedAt.Date <= request.To);
        
        if (request.Number > 0)
            query = query.Where(x => x.Number == request.Number);

        if (request.Status > 0)
            query = query.Where(x => x.Status == request.Status);

        return await query.Include(x => x.PaymentMethod)
            .Include(x=> x.ShippingAddress)
            .Include(x=> x.Collaborator)
            .Include(x=>x.Items)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}