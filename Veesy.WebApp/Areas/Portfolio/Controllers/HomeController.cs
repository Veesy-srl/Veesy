using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;

namespace Veesy.WebApp.Areas.Portfolio.Controllers;

[Area("Portfolio")]
[Authorize]
public class HomeController : VeesyController
{
    private readonly HomeHelper _homeHelper;
    public HomeController(UserManager<MyUser> userManager, IConfiguration config, HomeHelper homeHelper) : base(userManager, config)
    {
        _homeHelper = homeHelper;
    }
    
    [HttpGet("desk")]
    public IActionResult Index()
    {
        try
        {
            var vm = _homeHelper.GetDashboardViewModel(UserInfo);
            return View(vm);
        }
        catch (Exception e)
        {
            return RedirectToAction("Error400", "Public", new { area = "Public" });
        }
    }

}
