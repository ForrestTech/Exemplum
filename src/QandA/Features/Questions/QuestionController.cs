using Microsoft.AspNetCore.Mvc;

namespace QandA.Features.Questions
{
	public class QuestionController : Controller
	{
		[HttpGet("questions")]
		public IActionResult Index()
		{
			return Ok();
		}

		[HttpPost("questions")]
		public IActionResult Create()
		{
			return Ok();
		}
	}
}