using WebApi.Domain.Common;
using WebApi.Domain.Constants;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities.Subscriptions;

public class Subscription : BaseStoreEntity
{
    public Subscription()
    {
        
    }

    public Subscription(int storeId, int? referralId, decimal amount)
    {
        StoreId = storeId;
        Plan = "Plano Padrão";
        Amount = amount;
        ReferralId = referralId;
        CreatedAt = DateTime.Now;
        NextDueDate = DateTime.Now.AddDays(7);
        Status = StoreStatusConst.Active;
    }
    public decimal Amount { get; set; }
    public string? Plan { get; set; }
    public int Status { get; set; }
    public int? ReferralId { get; set; }
    public DateTime NextDueDate { get; set; }
    
    public ICollection<SubscriptionBilling>? Billings { get; set; }
}