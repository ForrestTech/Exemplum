namespace Exemplum.Application.Todo.Commands;

using Common.Security;
using Persistence;

[Authorize(Policy = Security.Policy.CanWriteTodo)]
public record MarkTodoCompleteCommand(int ListId, int TodoId) : 
    IRequest<OneOf<Success, NotFound, ValidationFailed>>
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

public class MarkTodoCompleteHandler :
    IRequestHandler<MarkTodoCompleteCommand, OneOf<Success, NotFound, ValidationFailed>>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<MarkTodoCompleteCommand> _validator;

    public MarkTodoCompleteHandler(IApplicationDbContext context, 
        IValidator<MarkTodoCompleteCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<OneOf<Success, NotFound, ValidationFailed>> Handle(MarkTodoCompleteCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (validation.IsInvalid())
        {
            return validation.ToFailure();
        }
        
        var todo = await _context.TodoItems
            .SingleOrDefaultAsync(x => x.ListId == request.ListId &&
                                       x.Id == request.TodoId, cancellationToken);

        if (todo == null)
        {
            return new NotFound();
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

        return new Success();
    }
}