using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project3.Areas.System.Controllers
{
    [Authorize(Policy = "AuthorizeSystemAreas")]
    [Area("system")]
    [Route("system")]
    public class DashboardController : Controller
    {
        [Route("dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
