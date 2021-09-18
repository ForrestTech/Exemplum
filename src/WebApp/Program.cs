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
        // TODO add colour picker for todo list will need to update domain logic to handle it
        // TODO add Confirm dialog for delete and snackbar notification
        // TODO link up all todo list actions to API
        // TODO migrate to using fluent validation https://blog.stevensanderson.com/2019/09/04/blazor-fluentvalidation/
        // TODO migrate to teal palette 
        // TODO move create list to its own component
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
