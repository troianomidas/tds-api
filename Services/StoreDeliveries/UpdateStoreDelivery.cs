using WebApi.Domain.Entities;
using WebApi.Persistence;
using WebApi.Services.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Stores;

public record UpdateStoreDeliveryRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public bool HasWithdraw { get; set; }
    public bool HasDelivery { get; set; }
    public bool HasSchedule { get; set; }
    public bool HasFreeDelivery { get; set; }
    public bool HasDeliveryArea { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal FreeDeliveryFrom { get; set; }
    public int DeliveryTimeMin { get; set; }
    public int DeliveryTimeMax { get; set; }
    public int WithdrawTimeMin { get; set; }
    public int WithdrawTimeMax { get; set; }
}

public class UpdateStoreDeliveryRequestHandler : IRequestHandler<UpdateStoreDeliveryRequest, bool>
{
    private readonly AppDbContext _context;

    public UpdateStoreDeliveryRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateStoreDeliveryRequest request, CancellationToken cancellationToken)
    {
        StoreDelivery? delivery = await _context.StoreDeliveries.Where(x => x.StoreId == request.StoreId).FirstOrDefaultAsync(cancellationToken);
        if (delivery == null)
            throw new InvalidOperationException("Loja nao encontrada.");

        delivery.HasWithdraw = request.HasWithdraw;
        delivery.HasDelivery = request.HasDelivery;
        delivery.HasSchedule = request.HasSchedule;
        delivery.HasFreeDelivery = request.HasFreeDelivery;
        delivery.FreeDeliveryFrom = request.FreeDeliveryFrom;
        delivery.HasDeliveryArea = request.HasDeliveryArea;
        delivery.DeliveryFee = request.DeliveryFee;
        delivery.DeliveryTimeMin = request.DeliveryTimeMin;
        delivery.DeliveryTimeMax = request.DeliveryTimeMax;
        delivery.WithdrawTimeMin = request.WithdrawTimeMin;
        delivery.WithdrawTimeMax = request.WithdrawTimeMax;
        delivery.UpdatedAt = DateTimeUtils.Now();
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}