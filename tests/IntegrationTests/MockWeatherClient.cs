namespace Exemplum.IntegrationTests;

using Application.WeatherForecasts;
using Application.WeatherForecasts.Models;

public class MockWeatherClient : IWeatherForecastClient
{
    public Task<WeatherForecast> GetForecast(decimal lat, decimal lon, string appId,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new WeatherForecast {Name = "MockResult"});
    }
}