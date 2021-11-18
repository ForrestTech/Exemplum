namespace Exemplum.Summary.Todo;

public class TodoState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string ListId { get; set; } = string.Empty;

    public State CurrentState { get; set; }  

    public List<TodoSummary> Todo { get; set; } = new();
}