namespace Exemplum.WebApi;

using Serilog.Core;

public class LogConfiguration
{
    public static Logger CreateLogger()
    {
        var logConfiguration = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.Seq("http://localhost:5341")
#else
                .MinimumLevel.Information()
#endif
            // if you want to get rid of some of the noise of asp.net core logging uncomment this line
            //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) 
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithSpan()
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                .WithDefaultDestructurers()
                .WithDestructurers(new[] {new SqlExceptionDestructurer()})
                .WithDestructurers(new[] {new DbUpdateExceptionDestructurer()}))
            .Enrich.WithProperty("ApplicationName", "Exemplum.Api")
            .Enrich.WithProperty("Assembly", Assembly.GetExecutingAssembly().FullName)
            .WriteTo.Console()
            .WriteTo.Async(c => c.File(new RenderedCompactJsonFormatter(), $"App_Data\\Logs\\ExemplumApi-Logs.txt",
                rollingInterval: RollingInterval.Day));

        return logConfiguration.CreateLogger();
    }
}