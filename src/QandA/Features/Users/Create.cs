using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QandA.Data;

namespace QandA.Features.Users
{
	public class Create : IRequestHandler<CreateUserRequest, User>
	{
		private readonly QandAContext _context;

		public Create(QandAContext context)
		{
			_context = context;
		}

		public async Task<User> Handle(CreateUserRequest request, CancellationToken cancellationToken)
		{
			var user = new User
			{
				Username = request.Username,
				Email = request.Email
			};

			_context.User.Add(user);
			await _context.SaveChangesAsync(cancellationToken);
			return user;
		}
	}

	public class CreateUserRequest : IRequest<User>
	{
		public string Username { get; set; }

		public string Email { get; set; }
	}
}