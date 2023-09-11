namespace Exemplum.Application.TodoList.Models;

using Domain.Todo;

public class TodoListDto 
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Colour { get; set; } = string.Empty;
}

public static class TodoListExtensions
{
    public static TodoListDto MapToDto(this TodoList item)
    {
        return new TodoListDto { Id = item.Id, Title = item.Title, Colour = item.Colour?.ToString() ?? string.Empty };
    }
}