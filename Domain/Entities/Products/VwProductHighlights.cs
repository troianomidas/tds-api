namespace WebApi.Domain.Entities.Products;

public class VwProductHighlights 
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public string? Store { get; set; }
    public string? Product { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int Views { get; set; }
    public decimal UnitPrice { get; set; }
    public int DiscountType { get; set; }
    public decimal Discount { get; set; }
    public string? BeginAt { get; set; }
    public string? EndAt { get; set; }
}