namespace Exemplum.Application.Common.Caching
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IApplicationCache<TCacheItem> where TCacheItem : class
    {
        Task<TCacheItem> GetOrAddAsync(string key,
            Func<Task<TCacheItem>> factory,
            TimeSpan expireIn,
            bool? hideErrors = null,
            CancellationToken cancellationToken = default(CancellationToken)) => GetOrAddAsync(key, factory,
            new CacheEntryOptions { AbsoluteExpirationRelativeToNow = expireIn }, hideErrors,
            cancellationToken);
        
        Task<TCacheItem> GetOrAddAsync(string key,
            Func<Task<TCacheItem>> factory,
            CacheEntryOptions? options = null,
            bool? hideErrors = null,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<TCacheItem?> GetAsync(string key,
            bool? hideErrors = null,
            CancellationToken cancellationToken = default(CancellationToken));

        Task SetAsync(string key,
            TCacheItem item,
            CacheEntryOptions? options,
            bool? hideErrors = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}