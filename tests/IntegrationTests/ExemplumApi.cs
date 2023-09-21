namespace Exemplum.IntegrationTests;

using Application.WeatherForecasts;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

public class ExemplumApi : WebApplicationFactory<Program>
{
    private readonly Action<IServiceCollection>? _configFunction;
    protected ITestOutputHelper Output { get; set; }

    public ExemplumApi(ITestOutputHelper output, Action<IServiceCollection>? configFunction = null)
    {
        _configFunction = configFunction;
        Output = output;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddXUnit(Output);
        });

        builder.ConfigureAppConfiguration(config =>
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            var configuration = configBuilder.Build();

            config.AddConfiguration(configuration);
        });

        builder.ConfigureServices(config =>
        {
            //this is a very basic example of how you can mock 3rd party dependencies when running integration tests, this would also work fine with Moq
            config.AddTransient<IWeatherForecastClient, MockWeatherClient>();

            _configFunction?.Invoke(config);
        });

        builder.ConfigureServices(services =>
        {
            var sp = services.BuildServiceProvider();
            Seeder.SeedDatabase(sp, true).Wait();
        });

        return base.CreateHost(builder);
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}