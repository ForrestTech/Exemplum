namespace Exemplum.WebApi.Endpoints;

using Application.WeatherForecasts.Models;
using Application.WeatherForecasts.Query;

public static class WeatherForecastEndpoints
{
    public static void MapWeatherForecastEndpoints(this WebApplication app)
    {
        app.MapGet("api/weatherforecast", GetWeatherForecast());
    }

    private static Func<IMediator, decimal, decimal, Task<WeatherForecast>> GetWeatherForecast()
    {
        return async (mediator, lat, lon) => await mediator.Send(new GetWeatherForecastQuery {Lat = lat, Lon = lon});
    }
}