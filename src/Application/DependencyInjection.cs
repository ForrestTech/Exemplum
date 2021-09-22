namespace Exemplum.Application
{
    using Common.Behaviour;
    using FluentValidation;
    using MediatR;
    using MediatR.Pipeline;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using WeatherForecasts;
    using WeatherForecasts.Models;
    using WeatherForecasts.Query;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            
            
            services.AddTransient(typeof(IPipelineBehavior<GetWeatherForecastQuery, WeatherForecast>), typeof(CacheForecastBehaviour));
            
            services.Configure<WeatherForecastOptions>(configuration.GetSection(WeatherForecastOptions.Section));

            return services;
        }
    }
}