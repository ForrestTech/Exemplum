namespace Exemplum.Application.Todo.Queries;

using Common.Exceptions;
using Domain.Todo;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;

public class GetTodoItemByIdQuery : IRequest<TodoItemDto>
{
    public int ListId { get; set; }

    public int TodoId { get; set; }
}

public class GetTodoItemInListQueryValidator : AbstractValidator<GetTodoItemByIdQuery>
{
    public GetTodoItemInListQueryValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);

        RuleFor(x => x.TodoId).GreaterThan(0);
    }
}

public class GetTodoItemInListQueryHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItemDto>
{
    private readonly IApplicationDbContext _context;

    public GetTodoItemInListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItemDto> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        var todo = await _context.TodoItems
            .AsNoTracking()
            .Where(x => x.ListId == request.ListId && x.Id == request.TodoId)
            .Select(x => x.MapToDto())
            .SingleOrDefaultAsync(cancellationToken);

        if (todo == null)
        {
            throw new NotFoundException(nameof(TodoItem), new {listId = request.ListId, taskId = request.TodoId});
        }

        return todo;
    }
}