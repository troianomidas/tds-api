using WebApi.Domain.Common;

namespace WebApi.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public string ExternalId { get; set; }
    public string? ExternalParentId { get; set; }
    public string? Item { get; set; }
    public string? Description { get; set; }
    public int ProductId { get; set; }
    public int Amount { get; set; }
    public decimal UnitValue { get; set; }
    public virtual Order? Order { get; set; }
}