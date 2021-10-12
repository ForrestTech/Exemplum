namespace Exemplum.Application.TodoList.Commands
{
    using AutoMapper;
    using Common.Security;
    using Domain.Exceptions;
    using Domain.Todo;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Models;
    using Persistence;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    [Authorize]
    public class CreateTodoListCommand : IRequest<TodoListDto>
    {
        public string Title { get; set; } = string.Empty;

        public string? Colour { get; set; }
    }

    public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateTodoListCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Title).NotEmpty()
                .MaximumLength(300)
                .MustAsync(BeUniqueTitle)
                .WithMessage("Todo list title must be unique");

            RuleFor(x => x.Colour)
                .Must(x => x == null || Colour.IsValidColour(x))
                .WithMessage("{PropertyName} must be a valid Colour");
        }


        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            var todo = await _context.TodoLists
                .SingleOrDefaultAsync(l => l.Title == title, cancellationToken: cancellationToken);

            return todo == null;
        }
    }

    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, TodoListDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateTodoListCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TodoListDto> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var list = new TodoList(request.Title);

            if (request.Colour is not null)
            {
                list.Colour = Colour.From(request.Colour);
            }

            _context.TodoLists.Add(list);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TodoListDto>(list);
        }
    }
}