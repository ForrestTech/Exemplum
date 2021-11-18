using Exemplum.Summary;
using FluentEmail.Core.Defaults;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host, services) =>
    {
        services.AddFluentEmail("exemplum@email.com");

        if (host.Configuration.SendEmailsToSmtp())
        {
            services.AddSingleton<ISender>(x => new SmtpSender(new SmtpClient("localhost", 25)));
        }
        else
        {
            services.AddSingleton<ISender>(x => new SaveToDiskSender(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.FullName));
        }

        services.AddMassTransit(x =>
        {
            x.AddSagaStateMachine<TodoSummaryStateMachine, TodoState>()
                .InMemoryRepository();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddMassTransitHostedService(true);
        
        services.AddOpenTelemetryTracing(x =>
        {
            x.AddMassTransitInstrumentation();
            x.AddJaegerExporter();
        });
    });

builder.UseSerilog((host, log) =>
{
    if (host.HostingEnvironment.IsProduction())
    {
        log.MinimumLevel.Information();
    }
    else
    {
        log.MinimumLevel.Debug()
            .WriteTo.Seq("http://localhost:5341");
    }

    log.Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .Enrich.WithProperty("ApplicationName", "Exemplum.Summary")
        .Enrich.WithProperty("Assembly", Assembly.GetExecutingAssembly().FullName)
        .WriteTo.Console()
        .WriteTo.Async(c => c.File(new RenderedCompactJsonFormatter(), $"App_Data\\Logs\\ExemplumSummary-Logs.txt",
            rollingInterval: RollingInterval.Day));
});

builder.Build().Run();