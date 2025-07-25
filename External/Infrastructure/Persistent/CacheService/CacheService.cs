using Application.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace Persistent.CacheService;

internal class AppCacheService : ICacheService
{
    IMemoryCache memoryCache;

    public AppCacheService(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }

    public Task<T1?> GetAsync<T1>(string key, CancellationToken cancellationToken = default) where T1 : class
    {
        return Task.FromResult<T1?>(memoryCache.Get<T1>(key));
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        memoryCache.Remove(key);
        return Task.CompletedTask;
    }

    public Task SetAsync<T1>(string key, T1 value, CancellationToken cancellationToken = default) where T1 : class
    {
        memoryCache.Set<T1>(key, value, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(10),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
        });

        return Task.CompletedTask;
    }
}
