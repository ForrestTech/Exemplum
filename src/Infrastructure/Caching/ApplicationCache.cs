namespace Exemplum.Infrastructure.Caching
{
    using Application.Common.Caching;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ApplicationCache<TCacheItem> : IApplicationCache<TCacheItem> where TCacheItem : class
    {
        private readonly DistributedCacheEntryOptions _defaultCacheEntryOptions;
        private readonly IDistributedCache _distributedCache;
        private readonly CacheOptions _options;
        private readonly ICacheSerializer _cacheSerializer;
        private readonly ILogger<ApplicationCache<TCacheItem>> _logger;
        private readonly SemaphoreSlim _syncSemaphore;

        public ApplicationCache(IDistributedCache distributedCache,
            IOptions<CacheOptions> options,
            ICacheSerializer cacheSerializer,
            ILogger<ApplicationCache<TCacheItem>> logger)
        {
            _distributedCache = distributedCache;
            _options = options.Value;
            _cacheSerializer = cacheSerializer;
            _logger = logger;

            _defaultCacheEntryOptions = GetDefaultCacheEntryOptions(_options);

            _syncSemaphore = new SemaphoreSlim(1, 1);
        }

        public async Task<TCacheItem> GetOrAddAsync(string key,
            Func<Task<TCacheItem>> factory,
            DistributedCacheEntryOptions? options = null,
            bool? hideErrors = null,
            CancellationToken cancellationToken = default)
        {
            hideErrors ??= _options.HideErrors;

            // if we get the value from the cache return straight away
            var value = await GetAsync(key, hideErrors, cancellationToken);
            if (value != null)
            {
                return value;
            }

            //take a lock so only one factory function can be called in the app.  
            //This is a naive lock that will lock any write access to the Cache for a given CacheItem 
            //possible performance could be improved by locking per key for large key spaces for a given CacheItem
            await _syncSemaphore.WaitAsync(cancellationToken);

            try
            {
                //safety check again if the item was added to the cache while we were getting lock
                value = await GetAsync(key, hideErrors, cancellationToken);
                if (value != null)
                {
                    return value;
                }

                //if we got here we now use the factory to get the cache item
                value = await factory();

                //cache it 
                await SetAsync(key, value, options, hideErrors, cancellationToken);
            }
            finally
            {
                //release our lock now others can write to the cache for CacheItem
                _syncSemaphore.Release();
            }

            return value;
        }

        public async Task<TCacheItem?> GetAsync(string key,
            bool? hideErrors = null,
            CancellationToken cancellationToken = default)
        {
            hideErrors ??= _options.HideErrors;

            byte[]? cachedBytes;

            try
            {
                cachedBytes = await _distributedCache.GetAsync(NormalizeKey(key), cancellationToken);
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    HandleException(ex);
                    return null;
                }

                throw;
            }

            return cachedBytes != null
                ? _cacheSerializer.Deserialize<TCacheItem>(cachedBytes)
                : null;
        }

        public async Task SetAsync(string key,
            TCacheItem item,
            DistributedCacheEntryOptions? options = null,
            bool? hideErrors = null,
            CancellationToken cancellationToken = default)
        {
            hideErrors ??= _options.HideErrors;

            try
            {
                var data = _cacheSerializer.Serialize(item);

                await _distributedCache.SetAsync(NormalizeKey(key), data, options ?? _defaultCacheEntryOptions,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                if (hideErrors == true)
                {
                    HandleException(ex);
                    return;
                }

                throw;
            }
        }

        private static DistributedCacheEntryOptions GetDefaultCacheEntryOptions(CacheOptions cacheOptions)
        {
            foreach (var configure in cacheOptions.CacheConfigurators)
            {
                var options = configure.Invoke(typeof(TCacheItem));
                if (options != null)
                {
                    return options;
                }
            }

            return cacheOptions.GlobalCacheEntryOptions;
        }

        private string NormalizeKey(string key)
        {
            return $"{_options.KeyPrefix}:{CacheName()}:{key}";
        }

        private static string CacheName()
        {
            return typeof(TCacheItem).Name;
        }

        private void HandleException(Exception ex)
        {
            _logger.LogError(ex, "An error occured trying to communicate with cache");
        }
    }
}