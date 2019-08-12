﻿using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using QandA.Data;

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
			RuleFor(x => x.Username).NotEmpty();
			RuleFor(x => x.Email).EmailAddress().NotEmpty();
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
			await _context.SaveChangesAsync(cancellationToken);
			return user;
		}
	}
}