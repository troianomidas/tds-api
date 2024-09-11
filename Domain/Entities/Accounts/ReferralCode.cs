using WebApi.Domain.Common;
using WebApi.Domain.Messages;

namespace WebApi.Domain.Entities;

public class ReferralCode : BaseEntity
{
    public string? Code { get; set; }
    public string? Seller { get; set; }
    public decimal Discount { get; set; }
    public DateTime ValidUntil { get; set; }
}