using WebApi.Domain.Common;

namespace WebApi.Domain.Entities;

public class ShippingAddress : BaseEntity
{
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? Number { get; set; }
    public string? Neighborhood { get; set; }
    public string? CityState { get; set; }
    public string? Zipcode { get; set; }
}