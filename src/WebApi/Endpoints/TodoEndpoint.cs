namespace Exemplum.WebApi.Endpoints;

using Application.Common.Pagination;
using Application.Todo.Commands;
using Application.Todo.Models;
using Application.Todo.Queries;
using Domain.Todo;
using Microsoft.AspNetCore.Http.HttpResults;

public class TodoEndpoints : IEndpoints
{
    private static readonly string Todo = nameof(Todo);
    
    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("api/todolist/{listId:int}/todo", GetTodoItemsInList)
            .WithSummary("Get todo items in a given list")
            .WithTags(Todo)
            .WithOpenApi();
        
        app.MapPost("api/todolist/{listId:int}/todo", CreateTodoItem)
            .WithSummary("Create todo item in a list")
            .WithTags(Todo)
            .WithOpenApi();
        
        app.MapGet("api/todolist/{listId:int}/todo/completed", GetCompletedTodoItems)
            .WithSummary("Get completed todo items in a list")
            .WithTags(Todo)
            .WithOpenApi();
        
        app.MapGet("api/todolist/{listId:int}/todo/{todoId:int}", GetTodoItemById)
            .WithName(nameof(GetTodoItemById))
            .WithSummary("Get a specific todo item by Id")
            .WithTags(Todo)
            .WithOpenApi();
        
        app.MapPut("api/todolist/{listId:int}/todo/{todoId:int}", UpdateTodoItem)
            .WithSummary("Update a specific todo item")
            .WithTags(Todo)
            .WithOpenApi();
        
        app.MapDelete("api/todolist/{listId:int}/todo/{todoId:int}", DeleteTodoItem)
            .WithSummary("Delete todo item")
            .WithTags(Todo)
            .WithOpenApi();
        
        app.MapPost("api/todolist/{listId:int}/todo/{todoId:int}/completed", MarkTodoItemCompleted)
            .WithSummary("Mark a todo item as complete")
            .WithTags(Todo)
            .WithOpenApi();
        
        app.MapPost("api/todolist/{listId:int}/todo/{todoId:int}/priority", SetTodoItemPriorityLevel)
            .WithSummary("Set the priority of a todo item")
            .WithTags(Todo)
            .WithOpenApi();
    }
    
    private static async Task<PaginatedList<TodoItemDto>> GetTodoItemsInList(ISender mediator,
        int listId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(
            new GetTodoItemsInListQuery {ListId = listId, PageNumber = pageNumber, PageSize = pageSize},
            cancellationToken);
    }
    
    private static async Task<CreatedAtRoute<TodoItemDto>> CreateTodoItem(ISender mediator,
        int listId,
        CreateTodoItemCommand command,
        CancellationToken cancellationToken = default)
    {
        command.ListId = listId;

        var result = await mediator.Send(command, cancellationToken);

        return TypedResults.CreatedAtRoute(result, nameof(GetTodoItemById), new {listId = command.ListId, todoId = result.Id});
    }
    
    private static async Task<PaginatedList<TodoItemDto>> GetCompletedTodoItems(ISender mediator,
        int listId,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(
            new GetCompletedTodoItemsQuery {ListId = listId, PageNumber = pageNumber, PageSize = pageSize},
            cancellationToken);
    }
    
    private static async Task<TodoItemDto> GetTodoItemById(ISender mediator,
        int listId,
        int todoId,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetTodoItemByIdQuery {ListId = listId, TodoId = todoId}, cancellationToken);
    }
    
    private static async Task<TodoItem> UpdateTodoItem(ISender mediator,
        int listId,
        int todoId,
        UpdateTodoCommand command,
        CancellationToken cancellationToken = default)
    {
        command.ListId = listId;
        command.TodoId = todoId;

        return await mediator.Send(command, cancellationToken);
    }
    
    private static async Task<IResult> DeleteTodoItem(ISender mediator,
        int listId,
        int todoId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteTodoCommand {ListId = listId, TodoId = todoId}, cancellationToken);

        return TypedResults.Ok();
    }
    
    private static async Task<IResult> MarkTodoItemCompleted(ISender mediator,
        int listId,
        int todoId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new MarkTodoCompleteCommand(listId, todoId), cancellationToken);
        return TypedResults.Ok();
    }
    
    private static async Task<IResult> SetTodoItemPriorityLevel(ISender mediator,
        int listId,
        int todoId,
        SetPriorityCommand command,
        CancellationToken cancellationToken = default)
    {
        command.ListId = listId;
        command.TodoId = todoId;

        await mediator.Send(command, cancellationToken);
        return TypedResults.Ok();
    }
}