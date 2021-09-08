namespace Application.TodoList.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Todo.Models;

    public class GetTodoItemInListQuery : IRequest<TodoItemDto?>
    {
        public int ListId { get; set; }

        public int TodoId { get; set; }
    }
    
    public class GetTodoItemInListQueryValidator : AbstractValidator<GetTodoItemInListQuery>
    {
        public GetTodoItemInListQueryValidator()
        {
            RuleFor(x => x.ListId).GreaterThan(0);
            
            RuleFor(x => x.TodoId).GreaterThan(0);
        }
    }
    
    public class GetTodoItemInListQueryHandler : IRequestHandler<GetTodoItemInListQuery, TodoItemDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTodoItemInListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<TodoItemDto?> Handle(GetTodoItemInListQuery request, CancellationToken cancellationToken)
        {
            var todo = await _context.TodoItems
                .AsNoTracking()
                .Where(x => x.ListId == request.ListId && x.Id == request.TodoId)
                .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            return todo;
        }
    }
}