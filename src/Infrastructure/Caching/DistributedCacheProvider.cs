namespace Exemplum.Infrastructure.Caching
{
    using Application.Common.Caching;
    using Microsoft.Extensions.Caching.Distributed;
    using System.Threading;
    using System.Threading.Tasks;

    public class DistributedCacheProvider : ICacheProvider
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCacheProvider(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public Task<byte[]?> GetAsync(string key, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _distributedCache.GetAsync(key, cancellationToken);
        }

        public Task SetAsync(string key, byte[] value, CacheEntryOptions options,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return _distributedCache.SetAsync(key, value, MapOptions(options), cancellationToken);
        }

        private static DistributedCacheEntryOptions MapOptions(CacheEntryOptions options)
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = options.AbsoluteExpiration,
                SlidingExpiration = options.SlidingExpiration,
                AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow
            };
        }
    }
}