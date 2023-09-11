namespace Exemplum.Application.Todo.Commands;

using Common.Exceptions;
using Common.Security;
using Domain.Todo;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;

[Authorize(Policy = Security.Policy.CanWriteTodo)]
public class CreateTodoItemCommand : IRequest<TodoItemDto>
{
    [JsonIgnore]
    public int ListId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Note { get; set; } = string.Empty;
}

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty()
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Note).MaximumLength(2000);
    }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, TodoItemDto>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItemDto> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var aggregate = await _context.TodoLists
            .Where(x => x.Id == request.ListId)
            .SingleOrDefaultAsync(cancellationToken);

        if (aggregate == null)
        {
            throw new NotFoundException(nameof(TodoItem), new {request.ListId});
        }

        var entity = new TodoItem(request.Title) {Note = request.Note};

        aggregate.AddToDo(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.MapToDto();
    }
}