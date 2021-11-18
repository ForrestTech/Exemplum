namespace Exemplum.IntegrationTests
{
    using Application.WeatherForecasts.Models;

    [Collection("ExemplumApiTests")]
    public class WeatherApiTests  
    {
        private readonly ITestOutputHelper _output;

        public WeatherApiTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public async Task WeatherForecast_get_returns_valid_forecast()
        {
            await using var application = new TodoAPI(_output);
            var client = application.CreateClient();
            
            var response = await client.GetAsync("api/weatherforecast?lat=11.96&lon=108.43");

            response.EnsureSuccessStatusCode();
            
            var actual = await response.Content.ReadFromJsonAsync<WeatherForecast>();

            actual?.Name.Should().Be("MockResult");
        }
    }
}