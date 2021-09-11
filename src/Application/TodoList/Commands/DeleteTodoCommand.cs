namespace Application.TodoList.Commands
{
    using Common.Exceptions;
    using Domain.Todo;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteTodoCommand : IRequest
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

    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTodoCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _context.TodoItems
                .SingleOrDefaultAsync(x => x.ListId == request.ListId && 
                                           x.Id == request.TodoId, cancellationToken);

            if (todo == null)
            {
                throw new NotFoundException(nameof(TodoItem), new { request.ListId, request.TodoId });
            }

            _context.TodoItems.Remove(todo);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}