using WebApi.Domain.Common;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities;

public class VerificationCode : BaseEntity
{
    public VerificationCode()
    {
        
    }
    
    public VerificationCode(string? publicIp, string? email)
    {
        PublicIp = publicIp;
        Email = email;
        Code = new Random().Next(111111, 999999).ToString();
        CreatedAt = DateTimeUtils.Now();
        ValidUntil = DateTimeUtils.Now().AddMinutes(30);
    }
    
    public string? Email { get; set; }
    public string? Code { get; set; }
    public string? PublicIp { get; set; }
    public DateTime ValidUntil { get; set; }
}