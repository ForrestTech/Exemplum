namespace Exemplum.WebApi.Endpoints;

using Application.Common.Pagination;
using Application.TodoList.Commands;
using Application.TodoList.Models;
using Application.TodoList.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

public class TodoListEndpoints : IEndpoints
{
    private static readonly string TodoList = nameof(TodoList);
    
    //todo add swagger meta data
    
    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("api/todolist", GetTodoLists)
            .WithSummary("Get todo lists")
            .WithTags(TodoList)
            .WithOpenApi();
        
        app.MapPost("api/todolist", CreateTodoList)
            .WithSummary("Create todo lists")
            .WithTags(TodoList)
            .WithOpenApi();
        
        
        app.MapGet("api/todolist/{listId:int}", GetTodoListById)
            .WithName(nameof(GetTodoListById))
            .WithSummary("Get todo list by ID")
            .WithTags(TodoList)
            .WithOpenApi();
        
        app.MapPut("api/todolist/{listId:int}", UpdateTodoList)
            .WithSummary("Update the todo list")
            .WithTags(TodoList)
            .WithOpenApi();
        
        app.MapDelete("api/todolist/{listId:int}", DeleteTodoList)
            .WithSummary("Delete the todo lists")
            .WithTags(TodoList)
            .WithOpenApi();
    }
    
    private static async Task<PaginatedList<TodoListDto>> GetTodoLists(ISender mediator,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetTodoListsQuery {PageNumber = pageNumber, PageSize = pageSize},
            cancellationToken);
    }
    
    private static async Task<CreatedAtRoute<TodoListDto>> CreateTodoList(ISender mediator,
        CreateTodoListCommand command,
        CancellationToken cancellationToken = default)
    {
        var item = await mediator.Send(command, cancellationToken);

        return TypedResults.CreatedAtRoute(item, nameof(GetTodoListById), new {listId = item.Id});
    }
    
    private static async Task<TodoListDto> UpdateTodoList(ISender mediator,
        int listId,
        UpdateTodoListCommand command,
        CancellationToken cancellationToken = default)
    {
        command.ListId = listId;

        return await mediator.Send(command, cancellationToken);
    }
    
    private static async Task<TodoListDto> GetTodoListById(ISender mediator,
        int listId,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetTodoListByIdQuery {ListId = listId}, cancellationToken);
    }
    
    private static async Task<IResult> DeleteTodoList(ISender mediator,
        int listId,
        CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteTodoListCommand {ListId = listId}, cancellationToken);
        return TypedResults.Ok();
    }
}