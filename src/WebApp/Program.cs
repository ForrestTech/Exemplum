namespace Exemplum.WebApp
{
    using Features.TodoLists;
    using Features.WeatherForecasts;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using MudBlazor.Services;
    using Refit;
    using Serilog;
    using Serilog.Events;
    using System;
    using System.Threading.Tasks;

    public class Program
    {
        // TODO create logo for exemplum
        // TODO create top right icons links for things like github
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

                builder.Services.AddSingleton<WeatherForecastService>();

                builder.Services.AddRefitClient<ITodoClient>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:5001"));
                
                builder.Services.AddMudServices();
                
                builder.Logging.AddSerilog();

                await builder.Build().RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An exception occurred while creating the WASM host");
                throw;
            }
        }
    }
}
