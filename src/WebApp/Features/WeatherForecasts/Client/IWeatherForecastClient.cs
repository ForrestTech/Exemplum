namespace Exemplum.WebApp.Features.WeatherForecasts.Client;

using Refit;

public interface IWeatherForecastClient
{
    [Get("/api/weatherforecast")]
    Task<WeatherForecast> GetForecast(double lat, double lon);
}