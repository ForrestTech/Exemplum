using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace QandA.Features.Users
{
	[ApiController]
	public class UserController : Controller
	{
		private readonly IMediator _mediator;

		public UserController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("users")]
		public async Task<IActionResult> Index([FromQuery]ListUsersRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("users")]
		public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("users/{userid}")]
		public async Task<IActionResult> Get([FromRoute]GetUserRequest request)
		{
			var response = await _mediator.Send(request);

			if (response != null)
			{
				return Ok(response);
			}

			return NotFound();
		}
	}
}