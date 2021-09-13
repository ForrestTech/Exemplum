namespace Application.WeatherForecast
{
    using Model;
    using Refit;
    using System.Threading.Tasks;

    public interface IWeatherForecastClient
    {
        [Get("/data/2.5/weather")]
        Task<WeatherForecast> GetForecast(decimal lat, decimal lon, string appId);
    }
}