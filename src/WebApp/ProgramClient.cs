namespace Exemplum.WebApp
{
    using Features.TodoLists.Client;
    using Features.WeatherForecasts.Client;
    using Location;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MudBlazor.Services;
    using Refit;
    using Serilog;
    using Serilog.Events;
    using System;
    using System.Threading.Tasks;

    public static class ProgramClient
    {
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

                builder.Logging.AddSerilog();

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
            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
                options.ProviderOptions.ResponseType = "code";
            });

            builder.Services.AddApiAuthorization().AddAccountClaimsPrincipalFactory<CustomUserFactory>();

            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("TodoWriteAccess", policy =>
                    policy.RequireClaim("permissions", "read:todo", "write:todo"));
                options.AddPolicy("TodoDeleteAccess", policy =>
                    policy.RequireClaim("permissions", "delete:todo"));
            });

            var apiHostUri = builder.Configuration.GetServiceUri("webapi") ?? ApiHostUri;

            builder.Services.AddRefitClient<ITodoClient>()
                .ConfigureHttpClient(c => c.BaseAddress = apiHostUri)
                .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                    .ConfigureHandler(new[] { apiHostUri.ToString() }));

            builder.Services.AddRefitClient<IWeatherForecastClient>()
                .ConfigureHttpClient(c => c.BaseAddress = apiHostUri)
                .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                    .ConfigureHandler(new[] { apiHostUri.ToString() }));

            builder.Services.AddTransient<ILocationService, LocationService>();
            builder.Services.AddMudServices();
        }

        private static readonly Uri ApiHostUri = new Uri("https://localhost:5001");
    }
}