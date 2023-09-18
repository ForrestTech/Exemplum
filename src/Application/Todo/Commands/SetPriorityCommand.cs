namespace Exemplum.Application.Todo.Commands;

using Common.Security;
using Domain.Common.DateAndTime;
using Domain.Extensions;
using Domain.Todo;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

[Authorize(Policy = Security.Policy.CanWriteTodo)]
public class SetPriorityCommand : 
    IRequest<OneOf<Success, NotFound, ValidationFailed>>
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
        RuleFor(x => x.PriorityLevel).Must(x => x.HasNoValue() || PriorityLevel.IsValid(x))
            .WithMessage("PriorityLevel is not a valid value");
    }
}

public class SetPriorityCommandHandler : 
    IRequestHandler<SetPriorityCommand, OneOf<Success, NotFound, ValidationFailed>>
{
    private readonly IApplicationDbContext _context;
    private readonly IClock _clock;
    private readonly IValidator<SetPriorityCommand> _validator;

    public SetPriorityCommandHandler(IApplicationDbContext context,
        IClock clock, 
        IValidator<SetPriorityCommand> validator)
    {
        _context = context;
        _clock = clock;
        _validator = validator;
    }

    public async Task<OneOf<Success, NotFound, ValidationFailed>> Handle(SetPriorityCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (validation.IsInvalid())
        {
            return validation.ToFailure();
        }
        
        var todo = await _context.TodoItems
            .Where(x => x.ListId == request.ListId && x.Id == request.TodoId)
            .SingleOrDefaultAsync(cancellationToken);

        if (todo == null)
        {
            return new NotFound();
        }
        
        todo.SetPriority(PriorityLevel.Parse(request.PriorityLevel), _clock);

        await _context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}