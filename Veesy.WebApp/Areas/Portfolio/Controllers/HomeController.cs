using Microsoft.AspNetCore.Mvc;

namespace Veesy.WebApp.Areas.Portfolio.Controllers
{
    [Area("Portfolio")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
