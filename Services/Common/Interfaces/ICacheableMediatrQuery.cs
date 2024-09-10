namespace WebApi.Services.Common.Interfaces;

public interface ICacheableMediatrQuery
{
    bool BypassCache { get; }
    string CacheKey { get; }
    TimeSpan? SlidingExpiration { get; }
}