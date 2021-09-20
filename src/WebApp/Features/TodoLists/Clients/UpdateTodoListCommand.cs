namespace Exemplum.WebApp.Features.TodoLists.Clients
{
    public record UpdateTodoListCommand(string Title, string? Colour)
    {
    }
}