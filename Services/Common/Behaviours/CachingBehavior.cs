using System.Text;
using WebApi.Services.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace WebApi.Services.Common.Behaviours;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableMediatrQuery
{
    private readonly IDistributedCache _cache;
    private readonly ILogger _logger;
    public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse? response;
        if (request.BypassCache) return await next();
        
        async Task<TResponse> GetResponseAndAddToCache()
        {
            response = await next();
            TimeSpan? slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromHours(3);
            var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };
            byte[] serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
            await _cache.SetAsync((string)request.CacheKey, serializedData, options, cancellationToken);
            return response;
        }
        
        byte[]? cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);
        if (cachedResponse != null)
        {
            response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
            _logger.LogInformation($"Fetched from Cache -> '{request.CacheKey}'.");
        }
        else
        {
            response = await GetResponseAndAddToCache();
            _logger.LogInformation($"Added to Cache -> '{request.CacheKey}'.");
        }
        
        return response;
    }
}