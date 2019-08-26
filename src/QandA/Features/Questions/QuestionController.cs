using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using X.PagedList;

namespace QandA.Features.Questions
{
	[ApiController]
	public class QuestionController : BaseController
	{
		private readonly IMediator _mediator;
		private readonly LinkGenerator _linkGenerator;

		public QuestionController(IMediator mediator, LinkGenerator linkGenerator)
		{
			_mediator = mediator;
			_linkGenerator = linkGenerator;
		}

		[HttpGet("questions")]
		public async Task<ActionResult<PagedList<Question>>> Index([FromQuery]ListQuestionRequest request)
		{
			var response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("questions/{questionId:int}")]
		public async Task<IActionResult> Get([FromRoute]GetQuestionRequest request)
		{
			var response = await _mediator.Send(request);

			if (response != null)
			{
				return Ok(response);
			}

			return NotFound();
		}

		[HttpPost("questions")]
		public async Task<IActionResult> Create([FromBody] CreateQuestionRequest request)
		{
			var response = await _mediator.Send(request);

			var url = _linkGenerator.GetPathByAction(nameof(Create), ControllerName(nameof(QuestionController)), new { userid = response.Id });

			return Created(url, response);
		}
	}
}