using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QandA.Data;

namespace QandA.Features.Users
{
	public class GetUserRequest : IRequest<User>
	{
		public int UserId { get; set; }
	}

	public class Get : IRequestHandler<GetUserRequest, User>
	{
		private readonly DatabaseContext _context;

		public Get(DatabaseContext context)
		{
			_context = context;
		}

		public Task<User> Handle(GetUserRequest request, CancellationToken cancellationToken)
		{
			return _context.Users.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
		}
	}
}