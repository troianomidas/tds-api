using WebApi.Domain.Constants;
using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Stores;

public record GetStoreByHostnameRequest : IRequest<Store?>
{
    public int StoreId { get; set; }
    public string? Hostname { get; set; }
}

public class GetStoreByHostnameRequestHandler : IRequestHandler<GetStoreByHostnameRequest, Store?>
{
    private readonly AppDbContext _context;

    public GetStoreByHostnameRequestHandler(AppDbContext context) => _context = context;

    public async Task<Store?> Handle(GetStoreByHostnameRequest request, CancellationToken cancellationToken)
    {
        return await _context.Stores.Where(x=> x.Hostname == request.Hostname && x.Status == StoreStatusConst.Active)
            .Include(x=> x.Address)
            .Include(x=> x.StoreDelivery)
            .Include(x=> x.StoreSettings)
            .Include(x=> x.OpeningHours)
            .Include(x=> x.StorePaymentMethods)
            .ThenInclude(x=> x.PaymentMethod)
            .FirstOrDefaultAsync(cancellationToken);
    }
}