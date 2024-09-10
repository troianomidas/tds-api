using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Stores;

public record GetStoreRequest : IRequest<Store?>
{
    public int StoreId { get; set; }
}

public class GetStoreRequestHandler : IRequestHandler<GetStoreRequest, Store?>
{
    private readonly AppDbContext _context;

    public GetStoreRequestHandler(AppDbContext context) => _context = context;

    public async Task<Store?> Handle(GetStoreRequest request, CancellationToken cancellationToken)
    {
        return await _context.Stores.Where(x=> x.Id == request.StoreId)
            .Include(x=> x.Address)
            .Include(x=> x.StoreSettings)
            .Include(x=> x.User)
            .FirstOrDefaultAsync(cancellationToken);
    }
}