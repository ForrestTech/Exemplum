namespace Exemplum.WebApp
{
    using Features.TodoLists;
    using Features.WeatherForecasts;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Refit;
    using Serilog;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("ApplicationName", "Exemplum.Web")
                .Enrich.WithProperty("Assembly", Assembly.GetExecutingAssembly().FullName)
                .WriteTo.BrowserConsole()
                .CreateLogger();

            try
            {
                Log.Information("Starting Web host");
                
                var builder = WebAssemblyHostBuilder.CreateDefault(args);
                builder.RootComponents.Add<App>("#app");

                builder.Services.AddSingleton<WeatherForecastService>();

                builder.Services.AddRefitClient<ITodoClient>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:5001"));
                
                builder.Logging.AddSerilog();

                await builder.Build().RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An exception occurred while creating the WASM host");
                throw;
            }
        }
        
//         public static async Task<int> Main(string[] args)
//         {
//             var logConfiguration = new LoggerConfiguration()
// #if DEBUG
//                 .MinimumLevel.Debug()
//                 .WriteTo.Debug()
//                 .WriteTo.Seq("http://localhost:5341")
// #else
//                 .MinimumLevel.Information()
// #endif
//                 // if you want to get rid of some of the noise of asp.net core logging uncomment this line
//                 //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) 
//                 .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
//                 .Enrich.FromLogContext()
//                 .Enrich.WithMachineName()
//                 .Enrich.WithEnvironmentName()
//                 .Enrich.WithSpan()
//                 .Enrich.WithProperty("ApplicationName", "Exemplum.Api")
//                 .Enrich.WithProperty("Assembly", Assembly.GetExecutingAssembly().FullName)
//                 .WriteTo.Console()
//                 .WriteTo.Async(c => c.File(new RenderedCompactJsonFormatter(), $"App_Data/Logs/ExemplumWeb-Logs-.txt",
//                     rollingInterval: RollingInterval.Day));
//
//             Log.Logger = logConfiguration.CreateLogger();
//             
//             try
//             {
//                 Log.Information("Starting web host");
//
//                 var host = CreateHostBuilder(args).Build();
//
//                 await host.RunAsync();
//
//                 return 0;
//             }
//             catch (Exception ex)
//             {
//                 Log.Fatal(ex, "Host terminated unexpectedly");
//                 return 1;
//             }
//             finally
//             {
//                 Log.CloseAndFlush();
//             }
//         }
//
//         public static IHostBuilder CreateHostBuilder(string[] args) =>
//             Host.CreateDefaultBuilder(args)
//                 .UseSerilog()
//                 .ConfigureWebHostDefaults(webBuilder =>
//                 {
//                     webBuilder.UseStartup<Startup>();
//                 });
    }
}
