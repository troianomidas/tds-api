using System.Globalization;
using WebApi.Domain.Common;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities;

public class StoreAddress : BaseStoreEntity
{
    public StoreAddress()
    {
        
    }

    public StoreAddress(string? zipcode, string? line1, string? number, string? neighborhood, string? cityState)
    {
        if (string.IsNullOrEmpty(zipcode))
            throw new InvalidOperationException("Por favor, informe o CEP do endereço da loja.");
        
        if (string.IsNullOrEmpty(line1))
            throw new InvalidOperationException("Por favor, informe o logradouro do endereço da loja.");
        
        if (string.IsNullOrEmpty(number))
            throw new InvalidOperationException("Por favor, informe o número do endereço da loja.");
        
        if (string.IsNullOrEmpty(neighborhood))
            throw new InvalidOperationException("Por favor, informe o bairro do endereço da loja.");
        
        if (string.IsNullOrEmpty(cityState))
            throw new InvalidOperationException("Por favor, informe a Cidade/Estado do endereço da loja.");
        
        TextInfo textInfo = new CultureInfo("pt-BR", false).TextInfo;
    
        Zipcode = zipcode.Trim();
        Line1 = textInfo.ToTitleCase(line1.ToLower().Trim());
        Number = number.Trim();
        Neighborhood = textInfo.ToTitleCase(neighborhood.ToLower().Trim());
        CityState = textInfo.ToTitleCase(cityState.ToLower().Trim());
        CreatedAt = DateTimeUtils.Now();
    }
    
    public string? Zipcode { get; set; }
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? Number { get; set; }
    public string? Neighborhood { get; set; }
    public string? CityState { get; set; }
}