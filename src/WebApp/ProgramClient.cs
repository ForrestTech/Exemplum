namespace Exemplum.WebApp
{
    using Features;
    using Features.TodoLists.Client;
    using Features.WeatherForecasts.Client;
    using Location;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using MudBlazor.Services;
    using Refit;
    using Serilog;
    using Serilog.Events;
    using System;
    using System.Threading.Tasks;

    public class ProgramClient
    {
        // TODO migrate to teal palette
        // TODO create logo for exemplum
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.BrowserConsole(LogEventLevel.Information)
                .CreateLogger();

            try
            {
                Log.Information("Starting Web host");

                var builder = WebAssemblyHostBuilder.CreateDefault(args);
                builder.RootComponents.Add<App>("#app");

                ConfigureServices(builder);

                await builder.Build().RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An exception occurred while creating the WASM host");
                throw;
            }
        }

        private static void ConfigureServices(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddRefitClient<ITodoClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(ApiHostUri));

            builder.Services.AddRefitClient<IWeatherForecastClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(ApiHostUri));

            builder.Services.AddTransient<ILocationService, LocationService>();
            builder.Services.AddMudServices();

            builder.Logging.AddSerilog();
        }

        private const string ApiHostUri = "https://localhost:5001";
    }
}