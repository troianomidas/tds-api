using WebApi.Domain.Common;

namespace WebApi.Domain.Entities;

public class RewardSystem : BaseStoreEntity 
{
    public int? OrderId { get; set; }
    public int? CustomerId { get; set; }
    public int? RewardId { get; set; }
    public decimal ValidPoints { get; set; }
    public decimal InvalidPoints { get; set; }
    public DateTime? ExpirationTerm { get; set; }
    public int MovType { get; set; }
    public virtual Reward? Reward { get; set; }
}