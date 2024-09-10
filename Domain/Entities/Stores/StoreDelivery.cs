using WebApi.Domain.Common;

namespace WebApi.Domain.Entities;

public class StoreDelivery : BaseStoreEntity
{
    public StoreDelivery()
    {
        
    }

    public StoreDelivery(int storeId, bool hasFreeDelivery, decimal deliveryFee)
    {
        StoreId = storeId;
        HasFreeDelivery = hasFreeDelivery;
        DeliveryFee = deliveryFee;
    }

    public bool HasWithdraw { get; set; }
    public bool HasDelivery { get; set; }
    public bool HasSchedule { get; set; }
    public bool HasDeliveryArea { get; set; }
    public bool HasFreeDelivery { get; set; }
    public decimal FreeDeliveryFrom { get; set; }
    public int DeliveryTimeMin { get; set; }
    public int DeliveryTimeMax { get; set; }
    public int WithdrawTimeMin { get; set; }
    public int WithdrawTimeMax { get; set; }
    public decimal DeliveryFee { get; set; }
    public DateTime UpdatedAt { get; set; }
}