namespace Exemplum.WebApp.Features.WeatherForecasts.Client;

public class WeatherForecast
{
    public string Name { get; set; } = string.Empty;

    public List<Weather> Weather { get; set; } = new();

    public Temperatures Main { get; set; } = new();
}