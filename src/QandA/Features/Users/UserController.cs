using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using X.PagedList;

namespace QandA.Features.Users
{
	[ApiController]
	public class UserController : BaseController
	{
		private readonly IMediator _mediator;
		private readonly LinkGenerator _linkGenerator;

		public UserController(IMediator mediator, LinkGenerator linkGenerator)
		{
			_mediator = mediator;
			_linkGenerator = linkGenerator;
		}

		[HttpGet("users")]
		public async Task<ActionResult<PagedList<User>>> Index([FromQuery]ListUsersRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("users/{userid:int}")]
		public async Task<IActionResult> Get([FromRoute]GetUserRequest request)
		{
			var response = await _mediator.Send(request);

			if (response != null)
			{
				return Ok(response);
			}

			return NotFound();
		}

		[HttpPost("users")]
		public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
		{
			var response = await _mediator.Send(request);

			var url = _linkGenerator.GetPathByAction(nameof(Create), ControllerName(nameof(UserController)),new { userid = response.Id });

			return Created(url, response);
		}
	}
}