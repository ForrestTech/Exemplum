namespace Exemplum.Application.WeatherForecasts.Models
{
    using System.Collections.Generic;

    public class WeatherForecast
    {
        public string Name { get; set; } = string.Empty;

        public List<Weather> Weather { get; set; } = new List<Weather>();

        public Temperatures Main { get; set; } = new Temperatures();
    }

    public class Weather
    {
        public string Main { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class Temperatures
    {
        public double Temp { get; set; }
    }
}