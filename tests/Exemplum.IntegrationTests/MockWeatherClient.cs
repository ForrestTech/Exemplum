namespace Application.IntegrationTests
{
    using Exemplum.Application.WeatherForecast;
    using Exemplum.Application.WeatherForecast.Model;
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