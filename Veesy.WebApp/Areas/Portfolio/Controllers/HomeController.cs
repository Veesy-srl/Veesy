using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veesy.WebApp.Areas.Portfolio.Controllers;

[Area("Portfolio")]
[Authorize]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Profile", "Profile", new { area = "Account" });
        return View();
    }
}
