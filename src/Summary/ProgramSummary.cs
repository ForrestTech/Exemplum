using Microsoft.Extensions.Hosting;

namespace Summary
{
    using FluentEmail.Core.Interfaces;
    using FluentEmail.Smtp;
    using MassTransit;
    using Microsoft.Extensions.DependencyInjection;
    using System.Net.Mail;

    public class ProgramSummary
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
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
    }
}