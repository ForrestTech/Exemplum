namespace Exemplum.WebApi.Endpoints;

using Application.WeatherForecasts.Models;
using Application.WeatherForecasts.Query;

public static class WeatherForecastEndpoints
{
    private static readonly string WeatherForecast = nameof(WeatherForecast);

    public static void MapWeatherForecastEndpoints(this WebApplication app)
    {
        app.MapGet("api/weatherforecast", GetWeatherForecast).WithTags(WeatherForecast);
    }

    private static Task<WeatherForecast> GetWeatherForecast(IMediator mediator, decimal lat, decimal lon)
    {
        return mediator.Send(new GetWeatherForecastQuery {Lat = lat, Lon = lon});
    }
}