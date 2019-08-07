using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace QandA.Controllers
{
	public class HomeController : Controller
	{
		private readonly IMediator _mediator;

		public HomeController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[Route("home/index")]
		public async Task<IActionResult> Index()
		{
			var response = await _mediator.Send(new HomeRequest());

			return Ok(response);
		}
	}

	public class HomeRequestHandler :IRequestHandler<HomeRequest, HomeResponse>
	{
		public Task<HomeResponse> Handle(HomeRequest request, CancellationToken cancellationToken)
		{
			return Task.FromResult(new HomeResponse
			{
				Message = "Hello from home controller"
			});
		}
	}

	public class HomeRequest : IRequest<HomeResponse>
	{

	}

	public class HomeResponse
	{
		public string Message { get; set; }
	}
}