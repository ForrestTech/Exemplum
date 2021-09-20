namespace Exemplum.WebApp.Features.TodoLists.Clients
{
    public record CreateTodoListCommand(string Title, string? Colour)
    {
    }
}