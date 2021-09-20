namespace Exemplum.WebApp
{
    using Features.TodoLists.Clients;
    using Features.WeatherForecasts;
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
        // TODO move create list to its own component
        // TODO review code to see if we need more seperation
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
            builder.Services.AddSingleton<WeatherForecastService>();

            builder.Services.AddRefitClient<ITodoClient>()
                .ConfigureHttpClient(c =>
                    c.BaseAddress = new Uri("https://localhost:5001")
                );

            builder.Services.AddMudServices();

            builder.Logging.AddSerilog();
        }
    }
}