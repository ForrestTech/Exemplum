using Microsoft.AspNetCore.Mvc;

namespace QandA.Features
{
	public class BaseController : Controller
	{
		protected static string ControllerName(string controllerClassName)
		{
			return controllerClassName.Replace("Controller","");
		}
	}
}