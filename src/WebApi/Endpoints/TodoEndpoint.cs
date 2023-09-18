namespace Exemplum.WebApi.Endpoints;

using Application.Common.Pagination;
using Application.Todo.Commands;
using Application.Todo.Models;
using Application.Todo.Queries;
using Domain.Todo;

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

    private static async Task<Results<Ok<PaginatedList<TodoItemDto>>, ValidationProblem>> GetTodoItemsInList(ISender mediator,
        int listId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result =  await mediator.Send(
            new GetTodoItemsInListQuery { ListId = listId, PageNumber = pageNumber, PageSize = pageSize },
            cancellationToken);

        return result.Match<Results<Ok<PaginatedList<TodoItemDto>>, ValidationProblem>>(
            dto => TypedResults.Ok(dto),
            failed => failed.ToValidationProblem());
    }

    private static async Task<Results<CreatedAtRoute<TodoItemDto>, NotFound, ValidationProblem>> CreateTodoItem(
        ISender mediator,
        int listId,
        CreateTodoItemCommand command,
        CancellationToken cancellationToken = default)
    {
        command.ListId = listId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<Results<CreatedAtRoute<TodoItemDto>, NotFound, ValidationProblem>>(
            dto => dto.ToCreatedAtRoute(nameof(GetTodoItemById), new { listId = command.ListId, todoId = dto.Id }),
            _ => TypedResults.NotFound(),
            failed => TypedResults.ValidationProblem(failed.Errors));
    }

    private static async Task<Results<Ok<PaginatedList<TodoItemDto>>, ValidationProblem>> GetCompletedTodoItems(ISender mediator,
        int listId,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result =  await mediator.Send(
            new GetCompletedTodoItemsQuery { ListId = listId, PageNumber = pageNumber, PageSize = pageSize },
            cancellationToken);

        return result.Match<Results<Ok<PaginatedList<TodoItemDto>>, ValidationProblem>>(
            dto => TypedResults.Ok(dto),
            failed => failed.ToValidationProblem());
    }

    private static async Task<Results<Ok<TodoItemDto>, NotFound, ValidationProblem>> GetTodoItemById(ISender mediator,
        int listId,
        int todoId,
        CancellationToken cancellationToken = default)
    {
        var result =  await mediator.Send(new GetTodoItemByIdQuery { ListId = listId, TodoId = todoId }, cancellationToken);

        return result.Match<Results<Ok<TodoItemDto>, NotFound, ValidationProblem>>(
            dto => TypedResults.Ok(dto),
            _ => TypedResults.NotFound(),
            failed => failed.ToValidationProblem());
    }

    private static async Task<Results<Ok<TodoItem>, NotFound, ValidationProblem>> UpdateTodoItem(ISender mediator,
        int listId,
        int todoId,
        UpdateTodoCommand command,
        CancellationToken cancellationToken = default)
    {
        command.ListId = listId;
        command.TodoId = todoId;

        var result = await mediator.Send(command, cancellationToken);
        
        return result.Match<Results<Ok<TodoItem>, NotFound, ValidationProblem>>(
            dto => TypedResults.Ok(dto),
            _ => TypedResults.NotFound(),
            failed => failed.ToValidationProblem());
    }

    private static async Task<Results<Ok, NotFound, ValidationProblem>> DeleteTodoItem(ISender mediator,
        int listId,
        int todoId,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new DeleteTodoCommand { ListId = listId, TodoId = todoId }, cancellationToken);

        return result.Match<Results<Ok, NotFound, ValidationProblem>>(
            _ => TypedResults.Ok(),
            _ => TypedResults.NotFound(),
            failed => failed.ToValidationProblem());
    }

    private static async Task<Results<Ok, NotFound, ValidationProblem>> MarkTodoItemCompleted(ISender mediator,
        int listId,
        int todoId,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new MarkTodoCompleteCommand(listId, todoId), cancellationToken);
        
        return result.Match<Results<Ok, NotFound, ValidationProblem>>(
            _ => TypedResults.Ok(),
            _ => TypedResults.NotFound(),
            failed => failed.ToValidationProblem());
    }

    private static async Task<Results<Ok, NotFound, ValidationProblem>> SetTodoItemPriorityLevel(ISender mediator,
        int listId,
        int todoId,
        SetPriorityCommand command,
        CancellationToken cancellationToken = default)
    {
        command.ListId = listId;
        command.TodoId = todoId;

        var result = await mediator.Send(command, cancellationToken);
        
        return result.Match<Results<Ok, NotFound, ValidationProblem>>(
            _ => TypedResults.Ok(),
            _ => TypedResults.NotFound(),
            failed => failed.ToValidationProblem());
    }
}