using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QandA.Data;
using X.PagedList;

namespace QandA.Features.Users
{
	public class ListUsersRequest : IRequest<PageWithMetaData<User>>, IPagedListRequest
	{
		public int PageNumber { get; set; } = 1;

		public int PageSize { get; set; } = 10;
	}

	public class List : IRequestHandler<ListUsersRequest, PageWithMetaData<User>>
	{
		private readonly DatabaseContext _context;

		public List(DatabaseContext context)
		{
			_context = context;
		}

		public async Task<PageWithMetaData<User>> Handle(ListUsersRequest request, CancellationToken cancellationToken)
		{
			var page = await _context.Users.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);

			return new PageWithMetaData<User>
			{
				Items = page,
				PageDetails = page.GetMetaData()
			};
		}
	}
}