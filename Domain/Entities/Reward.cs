using WebApi.Domain.Common;

namespace WebApi.Domain.Entities;

public class Reward : BaseStoreEntity 
{
    public decimal PointsCost { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public string? ImageUrl { get; set; }

    public virtual RewardSystem? RewardSystem { get; set; }
}