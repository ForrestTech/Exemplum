namespace Application.TodoList.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Persistence;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetTodoListQuery : IRequest<TodoListDto>
    {
        public int ListId { get; set; }
    }
    
    public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, TodoListDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTodoListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TodoListDto> Handle(GetTodoListQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.TodoLists
                .AsNoTracking()
                .Where(x => x.Id == request.ListId)
                .ProjectTo<TodoListDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}