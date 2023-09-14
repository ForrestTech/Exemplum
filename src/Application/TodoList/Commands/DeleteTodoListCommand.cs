namespace Exemplum.Application.TodoList.Commands;

using Common.Exceptions;
using Domain.Todo;
using Common.Security;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

public class DeleteTodoListCommand : IRequest<Unit>
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

public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    private readonly IRequestAuthorizationService _authorizationService;

    public DeleteTodoListCommandHandler(IRequestAuthorizationService authorizationService,
        IApplicationDbContext context)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    public async Task<Unit> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var list = await _context.TodoLists
            .SingleOrDefaultAsync(x => x.Id == request.ListId, cancellationToken);

        if (list == null)
        {
            throw new NotFoundException(nameof(TodoList), new {request.ListId});
        }

        await _authorizationService.AuthorizeRequestAsync<DeleteTodoListCommand>(list, Security.Policy.CanDeleteTodo);

        list.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}