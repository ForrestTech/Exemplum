using Exemplum.WebApp;
using Exemplum.WebApp.Features.TodoLists.Client;
using Exemplum.WebApp.Features.WeatherForecasts.Client;
using Exemplum.WebApp.Location;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Refit;
using Serilog;
using Serilog.Events;

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


static void ConfigureServices(WebAssemblyHostBuilder builder)
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
            policy.RequireClaim("permissions", "write:todo"));
        options.AddPolicy("TodoDeleteAccess", policy =>
            policy.RequireClaim("permissions", "delete:todo"));
    });

    var apiHostUri = new Uri("https://localhost:5001");

    builder.Services.AddRefitClient<ITodoClient>()
        .ConfigureHttpClient(c => c.BaseAddress = apiHostUri)
        .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(new[] {apiHostUri.ToString()}));

    builder.Services.AddRefitClient<IWeatherForecastClient>()
        .ConfigureHttpClient(c => c.BaseAddress = apiHostUri)
        .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(new[] {apiHostUri.ToString()}));

    builder.Services.AddTransient<ILocationService, LocationService>();
    builder.Services.AddMudServices();
}