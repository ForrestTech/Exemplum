using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using QandA.Data;
using X.PagedList;

namespace QandA.Features.Users
{
	public class ListUsersRequest : IRequest<PageWithMetaData<UserDTO>>, IPagedListRequest
	{
		public int PageNumber { get; set; } = 1;

		public int PageSize { get; set; } = 10;
	}

	public class List : IRequestHandler<ListUsersRequest, PageWithMetaData<UserDTO>>
	{
		private readonly DatabaseContext _context;
		private readonly IMapper _mapper;

		public List(DatabaseContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<PageWithMetaData<UserDTO>> Handle(ListUsersRequest request, CancellationToken cancellationToken)
		{
			var page = await _context.Users
				.ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
				.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);

			return page.ToPagedListWithMetaData();
		}
	}
}