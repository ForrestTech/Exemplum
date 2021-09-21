namespace Exemplum.Infrastructure
{
    using Application.Common.DomainEvents;
    using Application.Persistence;
    using Application.WeatherForecast;
    using DateAndTime;
    using Domain.Common.DateAndTime;
    using DomainEvents;
    using ExecutionPolicies;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;
    using Persistence.ExceptionHandling;
    using Polly.Caching;
    using Polly.Caching.Memory;
    using Polly.Registry;
    using Refit;
    using System;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
            IConfiguration configuration)
        {
            if (configuration.UseInMemoryDatabase())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("Exemplum"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options => 
                    options.UseSqlServer(configuration.GetDefaultConnection(),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);
            services.AddScoped<IEventHandlerDbContext>(provider => provider.GetService<ApplicationDbContext>()!);

            services.AddTransient<IHandleDbExceptions, HandleDbExceptions>();
            services.AddTransient<IPublishDomainEvents, DomainEventsPublisher>();
            services.AddTransient<IClock, Clock>();
            
            services.AddMemoryCache();
            services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();
            
            services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>(serviceProvider =>
            {
                var registry = new PolicyRegistry();
                ExecutionPolicyFactory.RegisterCachingPolicy(registry, serviceProvider, TimeSpan.FromMinutes(5));
                ExecutionPolicyFactory.RegisterRetryPolicy(registry, serviceProvider);
                return registry;
            });

            services.AddRefitClient<IWeatherForecastClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration
                        .GetSection($"{WeatherForecastOptions.Section}:{WeatherForecastOptions.BaseAddress}").Value))
                .AddPolicyHandlerFromRegistry(Exemplum.Application.Common.Policies.ExecutionPolicy.RetryPolicy);

            return services;
        }

      
    }
}