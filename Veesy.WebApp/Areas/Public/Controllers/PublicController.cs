using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Model.Cloud;

namespace Veesy.WebApp.Areas.Public.Controllers;

[Area("Public")]
public class PublicController : VeesyController
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly PublicHelper _publicHelper;
    private readonly IConfiguration _config;
    private readonly PortfolioHelper _portfolioHelper;
    private readonly INotyfService _notyfService;


    public PublicController(UserManager<MyUser> userManager, PublicHelper publicHelper, IConfiguration config, PortfolioHelper portfolioHelper, INotyfService notyfService) : base(
        userManager, config)
    {
        _publicHelper = publicHelper;
        _config = config;
        _portfolioHelper = portfolioHelper;
        _notyfService = notyfService;
    }
    
    [HttpGet("Contacts")]
    public IActionResult Contacts()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    
    [HttpGet("portfolio/{id}")]
    public IActionResult Portfolio(Guid id)
    {
        try
        {
            var result = _portfolioHelper.GetPortfolioPreviewViewModel(id,UserInfo);
            if (!result.result.Success)
            {
                _notyfService.Custom(result.result.Message, 10 , "#ca0a0a");
                return RedirectToAction("Index", "Home", new { area = "Portfolio" });
            }
            return View(result.model);
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            _notyfService.Custom("Error retrieving portfolios. Please retry.", 10 , "#ca0a0a");
            return RedirectToAction("Index", "Home", new { area = "Portfolio" });
        }
    }
    
    [HttpGet("Splash")]
    public IActionResult Splash()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    
    [HttpGet("Creators")]
    public IActionResult Creators()
    {
        try
        {
            var vm = _publicHelper.GetCreatorsViewModel();
            return View(vm);
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home", new { area = "Portfolio" });
        }
    }
    
    [HttpGet("FilterCreators")]
    public List<string> FilterCreators(string category)
    {
        return _publicHelper.GetCreatorsFiltered(category);
    }
    
    [HttpGet("About")]
    public IActionResult About()
    {
        try
        {
            var vm = _publicHelper.GetUserMediaList(7);
            return View(vm);

        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    
    [HttpGet("PricingPlan")]
    public IActionResult PricingPlan()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
}