namespace Application.WeatherForecast
{
    public class WeatherForecastOptions
    {
        public const string Section = "WeatherForecast";
        public const string BaseAddress = nameof(BaseAddress);
        
        public string AppId { get; set; } = string.Empty;
    }
}