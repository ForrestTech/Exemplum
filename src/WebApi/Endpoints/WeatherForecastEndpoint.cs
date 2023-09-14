namespace Exemplum.WebApi.Endpoints;

using Application.WeatherForecasts.Query;

public class WeatherForecastEndpoint  : IEndpoints
{
    private static readonly string WeatherForecast = nameof(WeatherForecast);

    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("api/weatherforecast", GetWeatherForecast)
            .WithTags(WeatherForecast);
    }

    private static Task<Application.WeatherForecasts.Models.WeatherForecast> GetWeatherForecast(IMediator mediator,
        decimal lat, 
        decimal lon)
    {
        return mediator.Send(new GetWeatherForecastQuery {Lat = lat, Lon = lon});
    }
}