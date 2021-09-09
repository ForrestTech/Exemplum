namespace Application.TodoList.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Exceptions;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Persistence;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetTodoListByIdQuery : IRequest<TodoListDto>
    {
        public int ListId { get; set; }
    }
    
    public class GetTodoListQueryValidator : AbstractValidator<GetTodoListByIdQuery>
    {
        public GetTodoListQueryValidator()
        {
            RuleFor(x => x.ListId).GreaterThan(0);
        }
    }
    
    public class GetTodoListQueryHandler : IRequestHandler<GetTodoListByIdQuery, TodoListDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTodoListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TodoListDto> Handle(GetTodoListByIdQuery request,
            CancellationToken cancellationToken)
        {
            var todoList = await _context.TodoLists
                .AsNoTracking()
                .Where(x => x.Id == request.ListId)
                .ProjectTo<TodoListDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            if (todoList == null)
            {
                throw new NotFoundException();
            }
    
            return todoList;
        }
    }
}