namespace Exemplum.Application.WeatherForecast.Model
{
    using System.Collections.Generic;

    public class WeatherForecast
    {
        public string Name { get; set; } = string.Empty;

        public List<Weather> Weather { get; set; } = new List<Weather>();

        public Main Main { get; set; } = new Main();
    }

    public class Weather
    {
        public string Main { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class Main
    {
        public double Temp { get; set; }
    }
}