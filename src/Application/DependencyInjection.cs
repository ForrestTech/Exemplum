namespace Exemplum.Application;

using Common.Behaviour;
using Common.Exceptions;
using Common.Exceptions.Converters;
using Common.Security;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using WeatherForecasts;
using WeatherForecasts.Models;
using WeatherForecasts.Query;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient<IExceptionToErrorConverter, ExceptionToErrorConverter>();
        services.AddTransient<ICustomExceptionErrorConverter, RefitApiExceptionConverter>();
        services.AddTransient<ICustomExceptionErrorConverter, UnauthorizedAccessExceptionConverter>();

        services.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddTransient(typeof(IPipelineBehavior<GetWeatherForecastQuery, WeatherForecast>),
            typeof(CacheForecastBehaviour));

        services.Configure<WeatherForecastOptions>(configuration.GetSection(WeatherForecastOptions.Section));

        services.AddTransient<IRequestAuthorizationService, RequestAuthorizationService>();
        services.AddSingleton<IAuthorizationHandler, OwnsResourceHandler>();

        services.AddAuthorizationCore(options =>
        {
            //permissions based policy
            options.AddPolicy(Security.Policy.CanWriteTodo,
                policy => policy.RequireClaim(Security.ClaimTypes.Permission, Security.Permissions.WriteTodo));
            
            //complex business logic policy
            options.AddPolicy(Security.Policy.CanDeleteTodo,
                policy => policy.Requirements.Add(new OwnsResourceRequirement()));
        });

        return services;
    }
}