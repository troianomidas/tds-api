using System.Text.Json.Serialization;
using WebApi.Services.Common.Interfaces;
using MediatR;

namespace WebApi.Services.Geolocation;

public record GetCitiesByName : IRequest<GetCitiesNearbyByLatLngResponse?>
{
    public string? Name { get; set; }
}

public class GetCitiesByNameHandler : IRequestHandler<GetCitiesByName, GetCitiesNearbyByLatLngResponse?>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string ApiKey = "b195bcfafamsh3a77f470f9431c3p1bd540jsn4272f68f7085";
    
    public GetCitiesByNameHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GetCitiesNearbyByLatLngResponse?> Handle(GetCitiesByName request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Name))
            throw new InvalidOperationException("City name is required.");
        
        HttpClient client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", ApiKey);
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");
        return await client.GetFromJsonAsync<GetCitiesNearbyByLatLngResponse?>($"https://wft-geo-db.p.rapidapi.com/v1/geo/cities?namePrefix={request.Name}&countryIds=BR&limit=10", cancellationToken: cancellationToken);
    }
}


