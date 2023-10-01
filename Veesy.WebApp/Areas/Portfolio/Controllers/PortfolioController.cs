using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;

namespace Veesy.WebApp.Areas.Portfolio.Controllers;

[Area("Portfolio")]
[Authorize]
public class PortfolioController : VeesyController
{
    private readonly PortfolioHelper _portfolioHelper;
    private readonly INotyfService _notyfService;

    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public PortfolioController(UserManager<MyUser> userManager, IConfiguration config,
        PortfolioHelper portfolioHelper, INotyfService notyfService) : base(userManager, config)
    {
        _portfolioHelper = portfolioHelper;
        _notyfService = notyfService;
    }
    
    [HttpGet("portfolios")]
    public IActionResult List()
    {
        return View();
    }
    
    [HttpGet("portfolio/settings")]
    public IActionResult Settings()
    {
        try
        {
            var vm = _portfolioHelper.GetPortfolioSettingsViewModel(UserInfo);
            return View(vm);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            _notyfService.Custom("Error retrieving portfolio settings. Please retry.", 10 , "#ca0a0a");
            return RedirectToAction("Index", "Home");
        }
    }
}