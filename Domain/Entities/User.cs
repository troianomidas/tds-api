using WebApi.Domain.Common;
using WebApi.Domain.Messages;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities;

public class User : BaseEntity
{
    public User()
    {
        
    }

    public User(string? email, string? pass, string? publicIp)
    {
        if (string.IsNullOrEmpty(email))
            throw new InvalidOperationException("Preencha o e-mail.", new Exception("email"));

        if (!email.Contains("@") || !email.Contains("."))
            throw new InvalidOperationException("Informe um e-mail válido.", new Exception("email"));

        if (string.IsNullOrEmpty(pass) || pass.Trim().Length < 8)
            throw new InvalidOperationException("Informe uma senha com no mínimo 8 caracteres.", new Exception("pass"));

        if (pass.Trim().Length > 45)
            throw new InvalidOperationException("Informe uma senha com no máximo 45 caracteres.", new Exception("pass"));
        
        Email = email.Trim();
        Password = pass;
        PublicIp = publicIp;
        CreatedAt = DateTime.Now;
        LastAccessAt = DateTime.Now;
    }
    
    public string? ExternalId { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public DateTime? LastAccessAt { get; set; }
    public string? PublicIp { get; set; }
}