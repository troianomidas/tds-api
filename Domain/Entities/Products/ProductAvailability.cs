using WebApi.Domain.Common;

namespace WebApi.Domain.Entities.Products;

public class ProductAvailability : BaseEntity
{
    public int ProductId { get; set; }
    public string? DayOfWeek { get; set; }
    public string? BeginAt { get; set; }
    public string? EndAt { get; set; }
    public int Sort { get; set; }
    
    public virtual Product? Product { get; set; }
}