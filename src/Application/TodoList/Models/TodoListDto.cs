namespace Exemplum.Application.TodoList.Models;

using Domain.Todo;

public class TodoListDto 
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Colour { get; init; } = string.Empty;
}

public static class TodoListExtensions
{
    public static TodoListDto MapToDto(this TodoList item)
    {
        return new TodoListDto {
            Id = item.Id, 
            Title = item.Title, 
            Colour = item.Colour?.Code ?? null 
        };
    }
}