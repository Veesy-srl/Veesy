using Microsoft.AspNetCore.Mvc;

namespace Veesy.WebApp.Areas.Account.Controllers;

[Area("Account")]
public class ProfileController : Controller
{
    [HttpGet("profile")]
    public IActionResult Profile()
    {
        return View();
    }
    
    [HttpGet("profile/basic-info")]
    public IActionResult BasicInfo()
    {
        return View();
    }
}