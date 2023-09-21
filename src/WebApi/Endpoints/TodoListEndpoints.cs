namespace Exemplum.WebApi.Endpoints;

using Application.Common.Pagination;
using Application.TodoList.Commands;
using Application.TodoList.Models;
using Application.TodoList.Queries;

public class TodoListEndpoints : IEndpoints
{
    private static readonly string TodoList = nameof(TodoList);

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

    private static async Task<Results<Ok<PaginatedList<TodoListDto>>, ValidationProblem>> GetTodoLists(ISender mediator,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetTodoListsQuery { PageNumber = pageNumber, PageSize = pageSize },
            cancellationToken);

        return result.Match<Results<Ok<PaginatedList<TodoListDto>>, ValidationProblem>>(
            dto => TypedResults.Ok(dto),
            failed => failed.ToValidationProblem());
    }

    private static async Task<Results<CreatedAtRoute<TodoListDto>, ValidationProblem>> CreateTodoList(ISender mediator,
        CreateTodoListCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<Results<CreatedAtRoute<TodoListDto>, ValidationProblem>>(
            dto => dto.ToCreatedAtRoute(nameof(GetTodoListById), new { listId = dto.Id }),
            failed => failed.ToValidationProblem());
    }

    private static async Task<Results<Ok<TodoListDto>, NotFound, ValidationProblem>> UpdateTodoList(ISender mediator,
        int listId,
        UpdateTodoListCommand command,
        CancellationToken cancellationToken = default)
    {
        command.ListId = listId;

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<Results<Ok<TodoListDto>, NotFound, ValidationProblem>>(
            dto => TypedResults.Ok(dto),
            _ => TypedResults.NotFound(),
            failed => failed.ToValidationProblem());
    }

    private static async Task<Results<Ok<TodoListDto>, NotFound, ValidationProblem>> GetTodoListById(ISender mediator,
        int listId,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetTodoListByIdQuery { ListId = listId }, cancellationToken);

        return result.Match<Results<Ok<TodoListDto>, NotFound, ValidationProblem>>(
            dto => TypedResults.Ok(dto),
            _ => TypedResults.NotFound(),
            failed => failed.ToValidationProblem());
    }

    private static async Task<Results<Ok, NotFound, UnauthorizedHttpResult, ValidationProblem>> DeleteTodoList(
        ISender mediator,
        int listId,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new DeleteTodoListCommand { ListId = listId }, cancellationToken);

        return result.Match<Results<Ok, NotFound, UnauthorizedHttpResult, ValidationProblem>>(
            _ => TypedResults.Ok(),
            _ => TypedResults.NotFound(),
            _ => TypedResults.Unauthorized(),
            failed => failed.ToValidationProblem());
    }
}