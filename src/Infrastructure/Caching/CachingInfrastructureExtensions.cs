namespace Exemplum.Infrastructure.Caching
{
    using Application.Common.Caching;
    using Application.WeatherForecast.Model;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;

    public static class CachingInfrastructureExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(IApplicationCache<>), typeof(ApplicationCache<>));

            if (configuration.UseInMemoryStorage())
            {
                services.AddMemoryCache();
                services.AddTransient<ICacheProvider, InMemoryCacheProvider>();
            }

            services.AddTransient<ICacheSerializer, Utf8JsonCacheSerializer>();

            services.Configure<CacheOptions>(options =>
            {
                options.KeyPrefix = "Exemplum";
                options.HideErrors = false;
                options.GlobalCacheEntryOptions = new CacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                var cacheItemConfigurators = new Dictionary<Type, CacheEntryOptions?>
                {
                    {
                        typeof(WeatherForecast),
                        new CacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) }
                    }
                };
                options.CacheConfigurators.Add(cacheItemType =>
                    cacheItemConfigurators.TryGetValue(cacheItemType, out CacheEntryOptions? cacheItemEntryOptions)
                        ? cacheItemEntryOptions
                        : null);
            });

            return services;
        }
    }
}