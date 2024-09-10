using WebApi.Domain.Common;

namespace WebApi.Domain.Entities;

public class PaymentMethod : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsOnline { get; set; }
    public decimal Fee { get; set; }
    public string? ImgUrl { get; set; }
}