using WebApi.Domain.Common;

namespace WebApi.Domain.Entities;

public class StorePaymentMethod : BaseStoreEntity
{
    public StorePaymentMethod()
    {
    }

    public StorePaymentMethod(int storeId, int paymentMethodId)
    {
        StoreId = storeId;
        PaymentMethodId = paymentMethodId;
    }

    public int PaymentMethodId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public virtual PaymentMethod? PaymentMethod { get; set; }
}