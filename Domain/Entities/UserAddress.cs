using WebApi.Domain.Common;

namespace WebApi.Domain.Entities;

public class UserAddress : BaseEntity
{
    public UserAddress()
    {
        
    }
    
    public UserAddress(string? zipcode, string? line1, string? number, string? neighborhood, string? cityState)
    {
        if (string.IsNullOrEmpty(zipcode))
            throw new InvalidOperationException("Por favor, preencha o campo 'CEP'.");
        
        if (string.IsNullOrEmpty(line1))
            throw new InvalidOperationException("Por favor, preencha o campo 'Logradouro'.");
        
        if (string.IsNullOrEmpty(number))
            throw new InvalidOperationException("Por favor, preencha o campo 'Número'.");
        
        if (string.IsNullOrEmpty(neighborhood))
            throw new InvalidOperationException("Por favor, preencha o campo 'Bairro'.");
        
        if (string.IsNullOrEmpty(cityState))
            throw new InvalidOperationException("Por favor, preencha o campo 'Cidade e Estado'.");

        Line1 = line1.Trim().ToUpper();
        Number = number.Trim().ToUpper();
        Neighborhood = neighborhood.Trim().ToUpper();
        CityState = cityState.Trim().ToUpper();
        Zipcode = zipcode.Trim();
        CreatedAt = DateTime.Now;
    }
    
    public int UserId { get; set; }
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? Number { get; set; }
    public string? Neighborhood { get; set; }
    public string? CityState { get; set; }
    public string? Zipcode { get; set; }
    public virtual User? User { get; set; }
}