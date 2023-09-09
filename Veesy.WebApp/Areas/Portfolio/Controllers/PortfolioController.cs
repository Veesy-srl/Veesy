using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veesy.WebApp.Areas.Portfolio.Controllers;

[Area("Portfolio")]
[Authorize]
public class PortfolioController : Controller
{
    [HttpGet("portfolios")]
    public IActionResult List()
    {
        return View();
    }
}