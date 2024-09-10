using WebApi.Domain.Common;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities;

public class WhatsappVerification : BaseEntity
{
    public WhatsappVerification()
    {
        
    }
    
    public WhatsappVerification(string? validationType, string? whatsappNumber)
    {
        ValidationType = validationType;
        WhatsappNumber = whatsappNumber;
        Code = new Random().Next(111111, 999999).ToString();
        CreatedAt = DateTimeUtils.Now();
        ValidUntil = DateTimeUtils.Now().AddHours(1);
    }
    
    public string? ValidationType { get; set; }
    public string? WhatsappNumber { get; set; }
    public string? Code { get; set; }
    public string? PublicIp { get; set; }
    public DateTime ValidUntil { get; set; }
}