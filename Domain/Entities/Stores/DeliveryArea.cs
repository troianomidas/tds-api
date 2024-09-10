using WebApi.Domain.Common;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities;

public class DeliveryArea : BaseStoreEntity
{
    public DeliveryArea()
    {
        
    }

    public DeliveryArea(int storeId, string? name, decimal fee)
    {
        StoreId = storeId;
        Name = name;
        Fee = fee;
        CreatedAt = DateTimeUtils.Now();
    }

    public string? Name { get; set; }
    public decimal Fee { get; set; }
}