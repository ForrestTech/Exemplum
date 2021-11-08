namespace Exemplum.Application.Todo.Commands;

using Common.Exceptions;
using Common.Security;
using Domain.Todo;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

[Authorize(Policy = Security.Policy.TodoWriteAccess)]
public class UpdateTodoCommand : IRequest<TodoItem>
{
    [JsonIgnore]
    public int ListId { get; set; }

    [JsonIgnore]
    public int TodoId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Note { get; set; } = string.Empty;
}

public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);
        RuleFor(x => x.TodoId).GreaterThan(0);
    }
}

public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, TodoItem>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = await _context.TodoItems
            .Where(x => x.ListId == request.ListId && x.Id == request.TodoId)
            .SingleOrDefaultAsync(cancellationToken);

        if (todo == null)
            throw new NotFoundException(nameof(TodoItem), new {request.ListId, request.TodoId});

        todo.Title = request.Title;
        todo.Note = request.Note;

        await _context.SaveChangesAsync(cancellationToken);

        return todo;
    }
}