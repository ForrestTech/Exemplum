namespace Exemplum.Application.Todo.Commands;

using Common.Security;
using Domain.Todo;
using Persistence;

public class DeleteTodoCommand : 
    IRequest<OneOf<Success, NotFound, ValidationFailed>>
{
    public int ListId { get; set; }

    public int TodoId { get; set; }
}

public class DeleteTodoCommandValidator : AbstractValidator<DeleteTodoCommand>
{
    public DeleteTodoCommandValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);
        RuleFor(x => x.TodoId).GreaterThan(0);
    }
}

public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, OneOf<Success, NotFound, ValidationFailed>>
{
    private readonly IRequestAuthorizationService _authorizationService;
    private readonly IApplicationDbContext _context;
    private readonly IValidator<DeleteTodoCommand> _validator;

    public DeleteTodoCommandHandler(IRequestAuthorizationService authorizationService,
        IApplicationDbContext context, 
        IValidator<DeleteTodoCommand> validator)
    {
        _authorizationService = authorizationService;
        _context = context;
        _validator = validator;
    }

    public async Task<OneOf<Success, NotFound, ValidationFailed>> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
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

        await _authorizationService.AuthorizeRequestAsync<DeleteTodoCommand>(todo, Security.Policy.CanDeleteTodo);

        _context.TodoItems.Remove(todo);

        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}