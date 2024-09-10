using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;

namespace WebApi.Services.Stores;

public record GetStoreAddress : IRequest<StoreAddress?>
{
    public int StoreId { get; set; }
}

public class GetStoreAddressHandler : IRequestHandler<GetStoreAddress, StoreAddress?>
{
    private readonly AppDbContext _context;

    public GetStoreAddressHandler(AppDbContext context) => _context = context;

    public async Task<StoreAddress?> Handle(GetStoreAddress request, CancellationToken cancellationToken)
    {
        return await _context.StoreAddresses.Where(x=> x.StoreId == request.StoreId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}