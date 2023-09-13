namespace Exemplum.Infrastructure;

using Application.Common.DomainEvents;
using Application.Common.Identity;
using Application.Common.IntegrationEvents;
using Application.Common.Policies;
using Application.Persistence;
using Application.WeatherForecasts;
using Caching;
using DateAndTime;
using Domain.Common.DateAndTime;
using DomainEvents;
using ExecutionPolicies;
using Identity;
using IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Persistence.ExceptionHandling;
using Polly.Registry;
using Refit;
using System;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        if (configuration.UseInMemoryStorage())
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite($"Data Source={GetDbPath()}"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetDefaultConnection()));
        }

        services.AddTransient<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);
        services.AddScoped<IEventHandlerDbContext>(provider => provider.GetService<ApplicationDbContext>()!);

        services.AddTransient<IHandleDbExceptions, HandleDbExceptions>();
        services.AddTransient<IPublishDomainEvents, DomainEventsPublisher>();
        services.AddTransient<IClock, Clock>();

        services.AddCaching(configuration);

        services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>(serviceProvider =>
        {
            var registry = new PolicyRegistry();
            ExecutionPolicyFactory.RegisterRetryPolicy(registry, serviceProvider);
            return registry;
        });

        services.AddRefitClient<IWeatherForecastClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration
                                                                  .GetSection(
                                                                      $"{WeatherForecastOptions.Section}:{WeatherForecastOptions.BaseAddress}")
                                                                  .Value ??
                                                              throw new InvalidOperationException()))
            .AddPolicyHandlerFromRegistry(ExecutionPolicy.RetryPolicy);

        if (environment.IsDevelopment())
        {
            services.AddTransient<ICurrentUser, FakeUser>();
        }
        else
        {
            services.AddTransient<ICurrentUser, CurrentUser>();
        }

        services.AddTransient<IUserIdentityService, UserIdentityService>();
        services.AddTransient<IExemplumAuthorizationService, ExemplumAuthorizationService>();

        return services;
    }

    private static string GetDbPath()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = Path.Join(path, "Exemplum.db");
        return dbPath;
    }
}