namespace Exemplum.Application
{
    using Common.Behaviour;
    using Common.ExecutionPolicies;
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
            //because refit auto generates a implementation we register this here not in infrastructure as there is no implementation at design time
            services.AddRefitClient<IWeatherForecastClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration.GetSection($"{WeatherForecastOptions.Section}:{WeatherForecastOptions.BaseAddress}").Value))
                .AddPolicyHandler(ExecutionPolicies.GetRetryPolicy());

            return services;
        }
        
       
    }
}