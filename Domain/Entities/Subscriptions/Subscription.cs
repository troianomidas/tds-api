using WebApi.Domain.Common;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities.Subscriptions;

public class Subscription : BaseStoreEntity
{
    public Subscription()
    {
        
    }

    public Subscription(int storeId, decimal amount)
    {
        StoreId = storeId;
        Plan = "Plano Padrão";
        Amount = amount;
        CreatedAt = DateTime.Now;
        NextDueDate = DateTime.Now.Date.AddMonths(1);
        Status = 1;
    }
    public decimal Amount { get; set; }
    public string? Plan { get; set; }
    public int Status { get; set; }
    public DateTime NextDueDate { get; set; }
    
    public ICollection<SubscriptionBilling>? Billings { get; set; }
}