namespace Application.IntegrationTests
{
    using FluentAssertions;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using WeatherForecast.Model;
    using Xunit;

    public partial class ExemplumApiTests : IClassFixture<WebHostFixture>
    {
        [Fact]
        public async Task WeatherForecast_get_returns_valid_forecast()
        {
            var response = await _client.GetAsync("api/weatherforecast?lat=11.96&lon=108.43");

            response.EnsureSuccessStatusCode();
            
            var actual = await response.Content.ReadFromJsonAsync<WeatherForecast>();

            actual?.Name.Should().Be("MockResult");
        }
    }
}