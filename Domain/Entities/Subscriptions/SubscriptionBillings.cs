using WebApi.Domain.Common;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities.Subscriptions;

public class SubscriptionBilling : BaseEntity
{
    public SubscriptionBilling()
    {
    }

    public SubscriptionBilling(int subscriptionId, int chargeId, string? billetLink, DateTime expireAt)
    {
        if (subscriptionId < 1)
            throw new InvalidOperationException("subscriptionId is required");
        
        if (chargeId < 1)
            throw new InvalidOperationException("chargeId is required");

        if (string.IsNullOrEmpty(billetLink))
            throw new InvalidOperationException("storeId is required");
        
        SubscriptionId = subscriptionId;
        ChargeId = chargeId;
        BilletLink = billetLink;
        ExpireAt = expireAt;
        CreatedAt = DateTime.Now;
        Status = 1;
    }

    public int SubscriptionId { get; set; }
    public int ChargeId { get; set; }
    public string? BilletLink { get; set; }
    
    public decimal Amount { get; set; }
    public int Status { get; set; }
    public DateTime ExpireAt { get; set; }
}