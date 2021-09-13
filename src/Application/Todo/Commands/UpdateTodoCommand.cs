namespace Application.Todo.Commands
{
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System.Linq;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateTodoCommand : IRequest
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
    
    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTodoCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _context.TodoItems
                .Where(x => x.ListId == request.ListId && x.Id == request.TodoId)
                .SingleOrDefaultAsync(cancellationToken);

            todo.Title = request.Title;
            todo.Note = request.Note;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}