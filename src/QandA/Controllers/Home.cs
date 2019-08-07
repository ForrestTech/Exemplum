using Microsoft.AspNetCore.Mvc;

namespace QandA.Controllers
{
	public class HomeController : Controller
	{
		[Route("home/index")]
		public IActionResult Index()
		{
			return Ok("Hello World from a controller");
		}
	}
}