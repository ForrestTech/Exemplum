namespace Exemplum.Application.TodoList.Commands
{
    using Common.Exceptions;
    using Common.Security;
    using Domain.Todo;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System.Threading;
    using System.Threading.Tasks;

    [Authorize]
    public class DeleteTodoListCommand : IRequest
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
    
    public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTodoListCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
        {
            var list = await _context.TodoLists
                .SingleOrDefaultAsync(x => x.Id == request.ListId, cancellationToken);

            if (list == null)
            {
                throw new NotFoundException(nameof(TodoList), new { request.ListId });
            }

            _context.TodoLists.Remove(list);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}