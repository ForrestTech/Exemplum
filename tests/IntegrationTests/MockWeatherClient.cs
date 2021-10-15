namespace Exemplum.IntegrationTests
{
    using Application.WeatherForecasts;
    using Application.WeatherForecasts.Models;
    using System.Threading;
    using System.Threading.Tasks;

    public class MockWeatherClient : IWeatherForecastClient
    {
        public Task<WeatherForecast> GetForecast(decimal lat, decimal lon, string appId,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new WeatherForecast { Name = "MockResult" });
        }
    }
}