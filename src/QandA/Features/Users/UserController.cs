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

		/// <summary>
		/// Get a list of users
		/// </summary>
		/// <param name="request">User list request</param>
		/// <returns>List of users</returns>
		[HttpGet("users")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<PagedList<User>>> Index([FromQuery]ListUsersRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		/// <summary>
		/// Get a specific User
		/// </summary>
		/// <param name="request">Get user request</param>
		/// <returns>User</returns>
		[HttpGet("users/{userid:int}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Get([FromRoute]GetUserRequest request)
		{
			var response = await _mediator.Send(request);

			if (response != null)
			{
				return Ok(response);
			}

			return NotFound();
		}

		/// <summary>
		/// Create a user for the Q and A system
		/// </summary>
		/// <param name="request">Create user request</param>
		/// <returns>Created User</returns>
		[HttpPost("users")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<User>> Create([FromBody] CreateUserRequest request)
		{
			var response = await _mediator.Send(request);

			var url = _linkGenerator.GetPathByAction(nameof(Create), ControllerName(nameof(UserController)),new { userid = response.Id });

			return Created(url, response);
		}
	}
}