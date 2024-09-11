using System.Globalization;
using WebApi.Domain.Common;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities;

public class StoreAddress : BaseStoreEntity
{
    public StoreAddress()
    {
        
    }

    public StoreAddress(string? zipcode, string? state, string? city, string? neighborhood, string? line1, string? number, string? line2)
    {
        if (string.IsNullOrEmpty(zipcode))
            throw new InvalidOperationException("Preencha o CEP.", new Exception("zipcode"));
        
        if(string.IsNullOrEmpty(city) || string.IsNullOrEmpty(state))
            throw new InvalidOperationException("Preencha corretamente o CEP para buscar o endereço.", new Exception("zipcode"));
        
        if (string.IsNullOrEmpty(neighborhood))
            throw new InvalidOperationException("Preencha o bairro.", new Exception("neighborhood"));
        
        if (!string.IsNullOrEmpty(neighborhood) && neighborhood.Length > 45)
            throw new InvalidOperationException("O bairro deve ter no máximo 45 caracteres.", new Exception("neighborhood"));
        
        if (string.IsNullOrEmpty(line1))
            throw new InvalidOperationException("Preencha o endereço.", new Exception("line1"));

        if (line1.Length is < 6 or > 100)
            throw new InvalidOperationException("O endereço deve ter entre 6 e 100 caracteres.", new Exception("line1"));
        
        if (string.IsNullOrEmpty(number))
            throw new InvalidOperationException("Preencha o número do endereço.", new Exception("number"));

        if (!string.IsNullOrEmpty(number) && number.Length > 45)
            throw new InvalidOperationException("O número do endereço deve ter no máximo 45 caracteres.", new Exception("number"));
        
        if (!string.IsNullOrEmpty(line2) && line2.Length > 45)
            throw new InvalidOperationException("O complemento ter no máximo 45 caracteres.", new Exception("line2"));
    
        Zipcode = zipcode.Trim();
        State = state.Trim();
        City = city.Trim();
        Neighborhood = neighborhood.Trim();
        Line1 = line1.Trim();
        Line2 = line2?.Trim();
        Number = number.Trim();
        CreatedAt = DateTimeUtils.Now();
    }
    
    public string? Zipcode { get; set; }
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? Number { get; set; }
    public string? Neighborhood { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
}