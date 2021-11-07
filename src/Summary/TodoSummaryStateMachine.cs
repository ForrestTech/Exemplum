namespace Summary
{
    using Automatonymous;
    using Exemplum.Domain.Todo.Events;
    using FluentEmail.Core;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;

    public class TodoSummaryStateMachine : MassTransitStateMachine<TodoState>
    {
        public TodoSummaryStateMachine(IFluentEmail email,
            ILogger<TodoSummaryStateMachine> logger)
        {
            InstanceState(x => x.CurrentState);

            Event(() => TodoItemCreated, x => x
                .CorrelateBy(state => state.ListId, context => context.Message.ListId.ToString())
                .SelectId(_ => Guid.NewGuid()));

            Initially(
                When(TodoItemCreated)
                    .Then(context =>
                    {
                        logger.LogInformation("Todo Summary state machine started");
                        context.Instance.ListId = context.Data.ListId.ToString();
                        context.Instance.Todo.Add(new TodoSummary {Title = context.Data.Title});
                    })
                    .TransitionTo(Created));

            During(Created,
                When(TodoItemCreated)
                    .IfElse(context => context.Instance.Todo.Count < 2,
                        x => x.Then(context =>
                        {
                            logger.LogInformation("Extra todo added to state machine");
                            context.Instance.Todo.Add(new TodoSummary {Title = context.Data.Title});
                        }),
                        x => x.ThenAsync(async context =>
                        {
                            logger.LogInformation("Email summary of recent tasks sent to user");

                            context.Instance.Todo.Add(new TodoSummary {Title = context.Data.Title});

                            var todoListSummary = string.Join(Environment.NewLine, context.Instance.Todo.Select(summary => summary.Title));
                            
                            await email
                                .To("user@exemplum.com", "User")
                                .Subject("Todo list summary")
                                .Body($"This is the summary of your tasks{Environment.NewLine}{todoListSummary}")
                                .SendAsync();
                        }).Finalize()
                    )
            );


            SetCompletedWhenFinalized();
        }

        public Event<TodoItemCreatedIntegrationEvent> TodoItemCreated { get; private set; }

        public State Created { get; private set; }
    }
}

namespace Exemplum.Domain.Todo.Events
{
    /// <summary>
    /// For mass transit to automatically map events the full type and namespace needs to match.  You could publish a contracts package that is shared by producers and consumers however, this does
    /// couple services together more than a simple copy paste.
    /// </summary>
    public class TodoItemCreatedIntegrationEvent
    {
        public string Title { get; set; } = string.Empty;

        public int ListId { get; set; }
    }
}