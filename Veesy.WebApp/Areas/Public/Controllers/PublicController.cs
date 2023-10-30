using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Model.Cloud;
using Veesy.Presentation.Model.Portfolio;
using Veesy.Service.Dtos;

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
    
    [HttpPost("FilterCreators")]
    public List<string> FilterCreators([FromBody] CategoryDto Category)
    {
        return _publicHelper.GetCreatorsFiltered(Category.Category);
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

    [HttpGet("portfolio/{id}")]
    public IActionResult Portfolio(Guid id)
    {
        try
        {
            var res = _portfolioHelper.GetPortfolioViewModel(id);
            if (!res.resultDto.Success)
            {
                _notyfService.Custom(res.resultDto.Message, 10, "#ca0a0a");
                return RedirectToAction("Error400");
            }

            return View(res.model);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return RedirectToAction("Error400");
        }
    }
    
    [HttpPost("portfolio/{id}")]
    public IActionResult Portfolio(PortfolioViewModel model)
    {
        try
        {
            var res = _portfolioHelper.GetPostPortfolioViewModel(model);
            if (!res.resultDto.Success)
            {
                _notyfService.Custom(res.resultDto.Message, 10, "#ca0a0a");
                return RedirectToAction("Error400");
            }
            
            if(!res.model.Unlocked && model.ControlPassword == 1)
                _notyfService.Custom("Insert password is not correct.", 10, "#ca0a0a");

            res.model.ControlPassword = 1;
            
            return View(res.model);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return RedirectToAction("Error400");
        }
    }
    
    [HttpGet("400")]
    public IActionResult Error400()
    {
        return View();
    }
}