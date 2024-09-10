namespace WebApi.Integrations.Serverless;

public class Delivery3Serverless : IDelivery3Serverless
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _baseUrl;
    private readonly string _code;

    public Delivery3Serverless(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _baseUrl = configuration["Serverless:BaseUrl"] ?? throw new InvalidOperationException();
        _code = configuration["Serverless:ApiKey"] ?? throw new InvalidOperationException();
    }

    public async Task RefreshWorkflowOrderAsync(int storeId)
    {
        HttpClient client = _httpClientFactory.CreateClient();
        try
        {
            await client.PostAsJsonAsync($"{_baseUrl}/api/RefreshWorkflowOrder?code={_code}", new { Id = storeId });
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR: " + e.Message + e.InnerException?.Message);
        }
    }
}