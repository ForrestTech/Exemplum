namespace Exemplum.WebApp.Features.WeatherForecasts.Client
{
    using System.Collections.Generic;

    public class WeatherForecast
    {
        public string Name { get; set; } = string.Empty;

        public List<Weather> Weather { get; set; } = new List<Weather>();

        public Temperatures Main { get; set; } = new Temperatures();
    }
}