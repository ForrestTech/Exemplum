namespace Exemplum.Application.Todo.Commands;

using Common.Exceptions;
using Common.Security;
using Domain.Todo;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

[Authorize(Policy = Security.Policy.TodoWriteAccess)]
public record MarkTodoCompleteCommand(int ListId, int TodoId) : IRequest
{
}

public class MarkTodoCompleteCommandValidator : AbstractValidator<MarkTodoCompleteCommand>
{
    public MarkTodoCompleteCommandValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);

        RuleFor(x => x.TodoId).GreaterThan(0);
    }
}

public class MarkTodoCompleteHandler : IRequestHandler<MarkTodoCompleteCommand>
{
    private readonly IApplicationDbContext _context;

    public MarkTodoCompleteHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(MarkTodoCompleteCommand request, CancellationToken cancellationToken)
    {
        var todo = await _context.TodoItems
            .SingleOrDefaultAsync(x => x.ListId == request.ListId &&
                                       x.Id == request.TodoId, cancellationToken);

        if (todo == null)
        {
            throw new NotFoundException(nameof(TodoItem), request);
        }

        if (todo.Done)
        {
            todo.MarkAsIncomplete();
        }
        else
        {
            todo.MarkAsDone();
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}