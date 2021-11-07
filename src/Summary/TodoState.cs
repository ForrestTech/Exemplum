namespace Summary
{
    using Automatonymous;
    using System;
    using System.Collections.Generic;

    public class TodoState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string ListId { get; set; }

        public State CurrentState { get; set; }

        public List<TodoSummary> Todo { get; set; } = new();
    }
}