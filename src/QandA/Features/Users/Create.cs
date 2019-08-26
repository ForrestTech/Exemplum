using System.Threading;
using System.Threading.Tasks;
using EntityFramework.Exceptions.Common;
using FluentValidation;
using MediatR;
using QandA.Data;
using QandA.Data.Configuration;

namespace QandA.Features.Users
{
	public class CreateUserRequest : IRequest<User>
	{
		public string Username { get; set; }

		public string Email { get; set; }
	}

	public class CreateUserValidator : AbstractValidator<CreateUserRequest>
	{
		public CreateUserValidator()
		{
			RuleFor(x => x.Username).NotEmpty()
				.MaximumLength(UserConfiguration.Constants.UsernameMaxLength);

			RuleFor(x => x.Email).EmailAddress()
				.NotEmpty()
				.MaximumLength(UserConfiguration.Constants.EmailMaxLength);
		}
	}

	public class Create : IRequestHandler<CreateUserRequest, User>
	{
		private readonly DatabaseContext _context;

		public Create(DatabaseContext context)
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

			_context.Users.Add(user);

			try
			{
				await _context.SaveChangesAsync(cancellationToken);
			}
			catch (UniqueConstraintException)
			{
				this.ThrowValidationException(nameof(CreateUserRequest.Username), $"{nameof(CreateUserRequest.Username)} must be unique");
			}

			return user;
		}
	}
}