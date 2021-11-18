using Summary.Todo;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddFluentEmail("exemplum@email.com");

        services.AddSingleton<ISender>(x => new SmtpSender(new SmtpClient("localhost", 25)));

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
    });

builder.Build().Run();