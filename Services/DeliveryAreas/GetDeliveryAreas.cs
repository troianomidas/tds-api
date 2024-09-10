using WebApi.Domain.Entities;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Stores;

public record GetDeliveryAreas : IRequest<List<DeliveryArea>>
{
    public int StoreId { get; set; }
}

public class GetDeliveryAreasRequestHandler : IRequestHandler<GetDeliveryAreas, List<DeliveryArea>>
{
    private readonly AppDbContext _context;

    public GetDeliveryAreasRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DeliveryArea>> Handle(GetDeliveryAreas request, CancellationToken cancellationToken)
    {
        return await _context.DeliveryAreas.Where(x => x.StoreId == request.StoreId).ToListAsync(cancellationToken);
    }
}