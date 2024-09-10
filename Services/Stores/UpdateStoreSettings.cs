using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Stores;

public record UpdateStoreSettings : IRequest<bool>
{
    public int StoreId { get; set; }
    public int FilterOrderDateType { get; set; }
    public int FilterOrderSortType { get; set; }
    public int FilterOrderSortAsc { get; set; }
    public decimal OrderMinValue { get; set; }
    public bool IsOpen { get; set; }
}

public class UpdateStoreSettingsHandler : IRequestHandler<UpdateStoreSettings, bool>
{
    private readonly AppDbContext _context;

    public UpdateStoreSettingsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateStoreSettings request, CancellationToken cancellationToken)
    {
        StoreSettings? settings = await _context.StoreSettings.Where(x => x.StoreId == request.StoreId)
            .FirstOrDefaultAsync(cancellationToken);

        if (settings == null)
            throw new InvalidOperationException("store settings not found.");
        
        settings.StoreId = request.StoreId;
        settings.FilterOrderDateType = request.FilterOrderDateType;
        settings.FilterOrderSortType = request.FilterOrderSortType;
        settings.FilterOrderSortAsc = request.FilterOrderSortAsc;
        settings.OrderMinValue = request.OrderMinValue;
        settings.IsOpen = request.IsOpen;
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}