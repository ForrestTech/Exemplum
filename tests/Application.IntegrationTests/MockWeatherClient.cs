namespace Application.IntegrationTests
{
    using System.Threading.Tasks;
    using WeatherForecast;
    using WeatherForecast.Model;

    public class MockWeatherClient : IWeatherForecastClient
    {
        public Task<WeatherForecast> GetForecast(decimal lat, decimal lon, string appId)
        {
            return Task.FromResult(new WeatherForecast { Name = "MockResult" });
        }
    }
}