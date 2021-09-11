namespace Application.TodoList.Commands
{
    using Common.Exceptions;
    using Domain.Todo;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

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
                                      x.Id == request.TodoId, cancellationToken: cancellationToken);

            if (todo == null)
            {
                throw new NotFoundException(nameof(TodoItem), request);
            }
            
            todo.MarkAsDone();

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}