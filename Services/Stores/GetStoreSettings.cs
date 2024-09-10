using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;

namespace WebApi.Services.Stores;

public record GetStoreSettings : IRequest<StoreSettings?>
{
    public int StoreId { get; set; }
}

public class GetStoreSettingsHandler : IRequestHandler<GetStoreSettings, StoreSettings?>
{
    private readonly AppDbContext _context;

    public GetStoreSettingsHandler(AppDbContext context) => _context = context;

    public async Task<StoreSettings?> Handle(GetStoreSettings request, CancellationToken cancellationToken)
    {
        return await _context.StoreSettings.Where(x=> x.StoreId == request.StoreId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}