using WebApi.Domain.Common;
using WebApi.Domain.Messages;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities;

public class User : BaseEntity
{
    public User()
    {
        
    }
    
    public User(string? name, string? whatsappNumber, string? pass1, string? pass2)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidOperationException("Informe seu nome completo.");

        if (name.Split(" ").Count() < 2)
            throw new InvalidOperationException("Informe seu nome completo.");
        
        if (name.Trim().Length > 65)
            throw new InvalidOperationException("Informe uma senha com no máximo 65 caracteres.");
        
        if (string.IsNullOrEmpty(whatsappNumber))
            throw new InvalidOperationException("Informe um número de WhatsApp válido.");
        
        if (whatsappNumber.Trim().Length < 14 || whatsappNumber.Trim().Length > 20)
            throw new InvalidOperationException("Informe um número de WhatsApp válido.");
        
        if (string.IsNullOrEmpty(pass1) || pass1.Trim().Length < 8)
            throw new InvalidOperationException("Informe uma senha com no mínimo 8 caracteres.");

        if (pass1.Trim().Length > 45)
            throw new InvalidOperationException("Informe uma senha com no máximo 45 caracteres.");

        if (pass2?.Trim() != pass1.Trim())
            throw new InvalidOperationException("As senhas não são iguais.");
        
        WhatsappNumber = whatsappNumber;
        Password = pass1;
        ExternalId = Guid.NewGuid().ToString();
        Name = name;
    }

    public User(string? name, string? document, string? email)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidOperationException("Por favor, informe seu nome completo.");

        if (name.Split(" ").Count() < 2)
            throw new InvalidOperationException("Por favor, informe seu nome completo.");
        
        if (name.Trim().Length > 65)
            throw new InvalidOperationException("Por favor, informe uma senha com no máximo 65 caracteres.");
        
        if (string.IsNullOrEmpty(email) || (!email.Contains("@") || !email.Contains(".com")))
            throw new InvalidOperationException("Por favor, informe um e-mail válido.");
 
        if (string.IsNullOrEmpty(document))
            throw new InvalidOperationException("Por favor, informe o CPF do responsável legal.");
        
        if (!CpfCnpj.IsCpfValid(document))
            throw new InvalidOperationException("Por favor, informe um CPF válido.");
        
        Name = name;
        Document = document;
        Email = email;
    }
    
    public string? Name { get; set; }
    public string? WhatsappNumber { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ExternalId { get; set; }
    public string? Document { get; set; }
    public DateTime? LastAccessAt { get; set; }
    public string? PublicIp { get; set; }
}