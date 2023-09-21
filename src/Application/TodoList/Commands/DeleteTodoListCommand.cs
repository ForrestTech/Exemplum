namespace Exemplum.Application.TodoList.Commands;

using Common.Security;
using Persistence;

public class DeleteTodoListCommand :
    IRequest<OneOf<Success, NotFound, Denied, ValidationFailed>>
{
    public int ListId { get; set; }
}

public class DeleteTodoListCommandValidator : AbstractValidator<DeleteTodoListCommand>
{
    public DeleteTodoListCommandValidator()
    {
        RuleFor(x => x.ListId).GreaterThanOrEqualTo(1);
    }
}

public class DeleteTodoListCommandHandler :
    IRequestHandler<DeleteTodoListCommand, OneOf<Success, NotFound, Denied, ValidationFailed>>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<DeleteTodoListCommand> _validator;

    private readonly IRequestAuthorizationService _authorizationService;

    public DeleteTodoListCommandHandler(IRequestAuthorizationService authorizationService,
        IApplicationDbContext context,
        IValidator<DeleteTodoListCommand> validator)
    {
        _context = context;
        _validator = validator;
        _authorizationService = authorizationService;
    }

    public async Task<OneOf<Success, NotFound, Denied, ValidationFailed>> Handle(DeleteTodoListCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsInvalid())
        {
            return validationResult.ToFailure();
        }

        var list = await _context.TodoLists
            .SingleOrDefaultAsync(x => x.Id == request.ListId, cancellationToken);

        if (list == null)
        {
            return new NotFound();
        }

        var authorised =
            await _authorizationService.AuthorizeRequestAsync(request, list, Security.Policy.CanDeleteTodo);

        if (authorised.Allowed is false)
        {
            return new Denied(authorised.DeniedReason);
        }

        list.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}