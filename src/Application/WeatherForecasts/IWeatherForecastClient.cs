namespace Exemplum.Application.WeatherForecasts;

using Models;
using Refit;

public interface IWeatherForecastClient
{
    [Get("/data/2.5/weather")]
    Task<WeatherForecast> GetForecast(decimal lat, decimal lon, string appId, CancellationToken cancellationToken);
}