namespace Exemplum.Infrastructure.Caching
{
    using Application.Common.Caching;
    using Application.WeatherForecast.Model;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;

    public static class CachingInfrastructureExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(IApplicationCache<>), typeof(ApplicationCache<>));
            
            services.AddTransient<ICacheSerializer, Utf8JsonCacheSerializer>();
            services.AddDistributedMemoryCache();
            
            if (!configuration.UseInMemoryStorage())
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = "localhost";
                });
            }

            services.Configure<CacheOptions>(options =>
            {
                options.KeyPrefix = "Exemplum";
                options.HideErrors = false;
                options.GlobalCacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                var cacheItemConfigurators = new Dictionary<Type, DistributedCacheEntryOptions?>
                {
                    {
                        typeof(WeatherForecast),
                        new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) }
                    }
                };
                options.CacheConfigurators.Add(cacheItemType =>
                    cacheItemConfigurators.TryGetValue(cacheItemType, out DistributedCacheEntryOptions? cacheItemEntryOptions)
                        ? cacheItemEntryOptions
                        : null);
            });

            return services;
        }
    }
}