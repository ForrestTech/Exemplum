using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QandA.Data;

namespace QandA.Features.Users
{
	public class GetUserRequest : IRequest<UserDTO>
	{
		public int UserId { get; set; }
	}

	public class Get : IRequestHandler<GetUserRequest, UserDTO>
	{
		private readonly DatabaseContext _context;
		private readonly IMapper _mapper;

		public Get(DatabaseContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<UserDTO> Handle(GetUserRequest request, CancellationToken cancellationToken)
		{
			var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

			return user != null ? _mapper.Map<UserDTO>(user) : null;
		}
	}
}