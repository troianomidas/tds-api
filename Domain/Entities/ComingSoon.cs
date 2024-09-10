using WebApi.Domain.Common;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities;

public class ComingSoon : BaseEntity
{
    public ComingSoon()
    {
        
    }

    public ComingSoon(string? name, string? phone, string? email)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidOperationException("O campo 'Nome' é obrigatório.");
        
        if (string.IsNullOrEmpty(phone))
            throw new InvalidOperationException("O campo 'Celular (com DDD)' é obrigatório.");
        
        if (string.IsNullOrEmpty(email))
            throw new InvalidOperationException("O campo 'E-mail' é obrigatório.");
        
        Name = name;
        Phone = phone;
        Email = email;
        CreatedAt = DateTimeUtils.Now();
    }
    
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}