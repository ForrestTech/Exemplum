using System;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QandA.Data;
using Serilog;
using Serilog.Events;

namespace QandA
{
	public class Program
	{
		public static int Main(string[] args)
		{
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var isDevelopment = environment == EnvironmentName.Development;

			var logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console();

			if (isDevelopment)
			{
				logger.WriteTo.Seq("http://localhost:5341");
			}
			else
			{
				logger.WriteTo.File(
					@"D:\home\LogFiles\Application\myapp.txt",
					fileSizeLimitBytes: 1_000_000,
					rollOnFileSizeLimit: true,
					shared: true,
					flushToDiskInterval: TimeSpan.FromSeconds(1));

				logger.WriteTo.ApplicationInsights(TelemetryConfiguration.Active, TelemetryConverter.Traces);
			}

			Log.Logger = logger.CreateLogger();

			try
			{
				Log.Information("Starting web host");

				var host = CreateWebHostBuilder(args).Build();

				MigrateDatabase(host);

				host.Run();

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

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseSerilog();

		private static void MigrateDatabase(IWebHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
				db.Database.Migrate();
			}
		}
	}
}
