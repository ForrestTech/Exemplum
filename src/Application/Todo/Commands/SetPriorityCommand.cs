namespace Exemplum.Application.Todo.Commands
{
    using Common.Exceptions;
    using Common.Security;
    using Domain.Common.DateAndTime;
    using Domain.Todo;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System;
    using System.Linq;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;

    [Authorize]
    public class SetPriorityCommand : IRequest
    {
        [JsonIgnore]
        public int ListId { get; set; }

        [JsonIgnore]
        public int TodoId { get; set; }

        public string PriorityLevel { get; set; } = String.Empty;
    }

    public class SetPriorityCommandValidator : AbstractValidator<SetPriorityCommand>
    {
        public SetPriorityCommandValidator()
        {
            RuleFor(x => x.ListId).GreaterThan(0);
            RuleFor(x => x.TodoId).GreaterThan(0);
            RuleFor(x => x.PriorityLevel).Must(x => PriorityLevel.TryFromValue(x, out _))
                .WithMessage("PriorityLevel is not a valid value");
        }
    }

    public class SetPriorityCommandHandler : IRequestHandler<SetPriorityCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IClock _clock;

        public SetPriorityCommandHandler(IApplicationDbContext context,
            IClock clock)
        {
            _context = context;
            _clock = clock;
        }

        public async Task<Unit> Handle(SetPriorityCommand request, CancellationToken cancellationToken)
        {
            var todo = await _context.TodoItems
                .Where(x => x.ListId == request.ListId && x.Id == request.TodoId)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            if (todo == null)
            {
                throw new NotFoundException(nameof(TodoItem), new { request.ListId, request.TodoId });
            }
            
            todo.SetPriority(PriorityLevel.FromValue(request.PriorityLevel), _clock);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}