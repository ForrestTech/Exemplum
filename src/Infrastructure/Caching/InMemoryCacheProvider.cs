namespace Exemplum.Infrastructure.Caching
{
    using Application.Common.Caching;
    using Microsoft.Extensions.Caching.Memory;
    using System.Threading;
    using System.Threading.Tasks;

    public class InMemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<byte[]?> GetAsync(string key, CancellationToken cancellationToken = default(CancellationToken))
        {
            var item = _memoryCache.Get(key);
            return (item == null ? Task.FromResult<byte[]?>(null)! : Task.FromResult((byte[])item))!;
        }

        public Task SetAsync(string key, byte[] value, CacheEntryOptions options,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            _memoryCache.Set(key, value, MapOptions(options));
            return Task.CompletedTask;
        }

        private static MemoryCacheEntryOptions MapOptions(CacheEntryOptions options)
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = options.AbsoluteExpiration,
                SlidingExpiration = options.SlidingExpiration,
                AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow
            };
        }
    }
}