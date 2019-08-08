using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QandA.Data;

namespace QandA.Features.Users
{
	public class Get : IRequestHandler<GetUserRequest, User>
	{
		private readonly QandAContext _context;

		public Get(QandAContext context)
		{
			_context = context;
		}

		public Task<User> Handle(GetUserRequest request, CancellationToken cancellationToken)
		{
			return _context.User.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
		}
	}

	public class GetUserRequest : IRequest<User>
	{
		public int UserId { get; set; }
	}
}