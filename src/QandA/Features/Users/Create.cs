using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EntityFramework.Exceptions.Common;
using FluentValidation;
using MediatR;
using QandA.Data;
using QandA.Data.Configuration;

namespace QandA.Features.Users
{
	public class CreateUserCommand : IRequest<UserDTO>
	{
		public string Username { get; set; }

		public string Email { get; set; }
	}

	public class CreateUserValidator : AbstractValidator<CreateUserCommand>
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

	public class Create : IRequestHandler<CreateUserCommand, UserDTO>
	{
		private readonly DatabaseContext _context;
		private readonly IMapper _mapper;

		public Create(DatabaseContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<UserDTO> Handle(CreateUserCommand command, CancellationToken cancellationToken)
		{
			var user = new User
			{
				Username = command.Username,
				Email = command.Email
			};

			_context.Users.Add(user);

			try
			{
				await _context.SaveChangesAsync(cancellationToken);
			}
			catch (UniqueConstraintException)
			{
				this.ThrowValidationException(nameof(CreateUserCommand.Username), $"{nameof(CreateUserCommand.Username)} must be unique");
			}

			return _mapper.Map<UserDTO>(user);
		}
	}
}