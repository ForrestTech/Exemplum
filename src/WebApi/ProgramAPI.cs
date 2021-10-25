namespace Exemplum.WebApi
{
    using Infrastructure.Persistence;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Enrichers.Span;
    using Serilog.Events;
    using Serilog.Exceptions;
    using Serilog.Exceptions.Core;
    using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
    using Serilog.Exceptions.MsSqlServer.Destructurers;
    using Serilog.Formatting.Compact;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class ProgramAPI
    {
        public static async Task<int> Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

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
                    .WithDestructurers(new[] { new SqlExceptionDestructurer() })
                    .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() }))
                .Enrich.WithProperty("ApplicationName", "Exemplum.Api")
                .Enrich.WithProperty("Assembly", Assembly.GetExecutingAssembly().FullName)
                .WriteTo.Console()
                .WriteTo.Async(c => c.File(new RenderedCompactJsonFormatter(), $"App_Data\\Logs\\ExemplumApi-Logs.txt",
                         rollingInterval: RollingInterval.Day));

            Log.Logger = logConfiguration.CreateLogger();

            try
            {
                Log.Information("Starting API host");

                var host = CreateHostBuilder(args).Build();

                await SeedDatabase(host);

                await host.RunAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static async Task SeedDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();

                if (context.Database.IsSqlServer())
                {
                    await context.Database.MigrateAsync();
                }

                await ApplicationDbContextSeed.SeedSampleDataAsync(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while migrating or seeding the database");

                throw;
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}