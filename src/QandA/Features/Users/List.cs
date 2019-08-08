using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QandA.Data;
using X.PagedList;

namespace QandA.Features.Users
{
	public class List : IRequestHandler<ListUsersRequest, IPagedList<User>>
	{
		private readonly DatabaseContext _context;

		public List(DatabaseContext context)
		{
			_context = context;
		}

		public Task<IPagedList<User>> Handle(ListUsersRequest request, CancellationToken cancellationToken)
		{
			return _context.Users.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
		}
	}

	public class ListUsersRequest : IRequest<IPagedList<User>>, IPagedListRequest
	{
		public int PageNumber { get; set; } = 1;

		public int PageSize { get; set; } = 10;
	}

}