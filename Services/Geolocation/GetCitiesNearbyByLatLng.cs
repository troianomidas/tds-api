using System.Text.Json.Serialization;
using WebApi.Services.Common.Interfaces;
using MediatR;

namespace WebApi.Services.Geolocation;

public record GetCitiesNearbyByLatLng : IRequest<GetCitiesNearbyByLatLngResponse?>, ICacheableMediatrQuery
{
    public string? Lat { get; set; }
    public string? Lng { get; set; }
    public bool BypassCache { get; set; }
    public string CacheKey => $"Lat-{Lat}-Lng-{Lng}";
    public TimeSpan? SlidingExpiration { get; set; }
}

public class GetCitiesNearbyByLatLngHandler : IRequestHandler<GetCitiesNearbyByLatLng, GetCitiesNearbyByLatLngResponse?>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string ApiKey = "b195bcfafamsh3a77f470f9431c3p1bd540jsn4272f68f7085";
    
    public GetCitiesNearbyByLatLngHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GetCitiesNearbyByLatLngResponse?> Handle(GetCitiesNearbyByLatLng request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Lat) || string.IsNullOrEmpty(request.Lat))
            throw new InvalidOperationException("Lat and Lng is required.");
        
        HttpClient client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", ApiKey);
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");
        return await client.GetFromJsonAsync<GetCitiesNearbyByLatLngResponse?>($"https://wft-geo-db.p.rapidapi.com/v1/geo/locations/{request.Lat}{request.Lng}/nearbyCities?radius=85&limit=10&distanceUnit=KM&offset=0", cancellationToken: cancellationToken);
    }
}

public class GetCitiesNearbyByLatLngResponse
{
    [JsonPropertyName("data")]
    public List<Datum>? Data { get; set; }
    
    public class Datum
    {
        [JsonPropertyName("city")]
        public string? City { get; set; }
        
        [JsonPropertyName("regionCode")]
        public string? RegionCode { get; set; }
        
        [JsonPropertyName("distance")]
        public double? Distance { get; set; }
        
        [JsonPropertyName("latitude")]
        public double? Lat { get; set; }
    
        [JsonPropertyName("longitude")]
        public double? Lng { get; set; }
    }
}


