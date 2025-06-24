using Microsoft.AspNetCore.Mvc;

namespace Project3.Controllers
{
	public class AboutUsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
