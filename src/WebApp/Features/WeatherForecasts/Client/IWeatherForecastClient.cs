namespace Exemplum.WebApp.Features.WeatherForecasts.Client
{
    using Refit;
    using System.Threading.Tasks;

    public interface IWeatherForecastClient
    {
        [Get("/api/weatherforecast")]
        Task<WeatherForecast> GetForecast(double lat, double lon);
    }
}