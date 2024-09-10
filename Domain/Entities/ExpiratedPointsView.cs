namespace WebApi.Domain.Entities;

public class ExpiratedPointsView
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int CustomerId { get; set; }
    public decimal ExpiratedPoints { get; set; }
    public DateTime ExpirationTerm { get; set; }
}