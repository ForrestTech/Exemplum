﻿namespace Application.IntegrationTests
{
    using Exemplum.Application.WeatherForecast;
    using Exemplum.Infrastructure.Persistence;
    using Exemplum.WebApi;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;
    using Xunit.Abstractions;

    public class WebHostFixture : WebApplicationFactory<Startup>
    {
        public ITestOutputHelper Output { get; set; }

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = base.CreateHostBuilder();
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
            });

            return builder;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();

                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<WebHostFixture>>();

                try
                {
                    ApplicationDbContextSeed.SeedSampleDataAsync(db).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                                        "database with test messages. Error: {Message}", ex.Message);
                }
            });
        }
    }
}