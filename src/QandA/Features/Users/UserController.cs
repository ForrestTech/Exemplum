using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace QandA.Features.Users
{
	public class UserController : Controller
	{
		private readonly IMediator _mediator;

		public UserController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("users")]
		public IActionResult Index()
		{
			return Ok();
		}

		[HttpPost("users")]
		public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("users/{userid}")]
		public IActionResult Get(int userid)
		{
			return Ok();
		}
	}
}