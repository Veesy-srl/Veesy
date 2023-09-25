using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;

namespace Veesy.WebApp.Areas.Portfolio.Controllers;

[Area("Portfolio")]
[Authorize]
public class PortfolioController : VeesyController
{
    private readonly PortfolioHelper _portfolioHelper;

    public PortfolioController(UserManager<MyUser> userManager, IConfiguration config,
        PortfolioHelper portfolioHelper) : base(userManager, config)
    {
        _portfolioHelper = portfolioHelper;
    }
    
    [HttpGet("portfolios")]
    public IActionResult List()
    {
        return View();
    }
    
    [HttpGet("portfolio/settings")]
    public IActionResult Settings()
    {
        var vm = _portfolioHelper.GetPortfolioSettingsViewModel(UserInfo);
        return View();
    }
}