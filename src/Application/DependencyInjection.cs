namespace Application
{
    using Common.Behaviour;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Refit;
    using System;
    using System.Reflection;
    using WeatherForecast;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.Configure<WeatherForecastOptions>(configuration.GetSection(WeatherForecastOptions.Section));
            services.AddRefitClient<IWeatherForecastClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration.GetSection($"{WeatherForecastOptions.Section}:{WeatherForecastOptions.BaseAddress}").Value));

            return services;
        }
    }
}