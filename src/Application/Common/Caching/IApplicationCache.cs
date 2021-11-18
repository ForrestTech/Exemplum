namespace Exemplum.Application.Common.Caching;

using Microsoft.Extensions.Caching.Distributed;

public interface IApplicationCache<TCacheItem> where TCacheItem : class
{
    Task<TCacheItem> GetOrAddAsync(string key,
        Func<Task<TCacheItem>> factory,
        TimeSpan expireIn,
        bool? hideErrors = null,
        CancellationToken cancellationToken = default) => GetOrAddAsync(key, factory,
        new DistributedCacheEntryOptions {AbsoluteExpirationRelativeToNow = expireIn}, hideErrors,
        cancellationToken);

    Task<TCacheItem> GetOrAddAsync(string key,
        Func<Task<TCacheItem>> factory,
        DistributedCacheEntryOptions? options = null,
        bool? hideErrors = null,
        CancellationToken cancellationToken = default);

    Task<TCacheItem?> GetAsync(string key,
        bool? hideErrors = null,
        CancellationToken cancellationToken = default);

    Task SetAsync(string key,
        TCacheItem item,
        DistributedCacheEntryOptions? options,
        bool? hideErrors = null,
        CancellationToken cancellationToken = default);
}