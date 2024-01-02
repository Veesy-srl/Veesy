using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
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
    private readonly SignInManager<MyUser> _signInManager;


    public PublicController(UserManager<MyUser> userManager, PublicHelper publicHelper, IConfiguration config, PortfolioHelper portfolioHelper, INotyfService notyfService, SignInManager<MyUser> signInManager) : base(
        userManager, config)
    {
        _publicHelper = publicHelper;
        _config = config;
        _portfolioHelper = portfolioHelper;
        _notyfService = notyfService;
        _signInManager = signInManager;
    }
    
    [HttpGet("contacts")]
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
    
    [HttpGet("cookies-policy")]
    public IActionResult CookiesPolicy()
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
    
    [HttpGet]
    public IActionResult Splash()
    {
        try
        {
            if (_signInManager.IsSignedIn(User)) //verify if it's logged
            {
                return RedirectToAction("Index", "Home", new { area = "Portfolio" });
            }
            return View();
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    
    [HttpGet("FontTest")]
    public IActionResult FontTest()
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
    
    [HttpGet("creators")]
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
    
    [HttpPost]
    public async Task<JsonResult> FilterCreators([FromBody] CategoryDto Category)
    {
        try
        {
            var result = _publicHelper.GetCreatorsFiltered(Category.Category);
            if (result.Count == 0)
            {
                return Json(new { Result = new List<string>() });
            }
            return Json(new { Result = result.ToList() });
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return Json(new { Result = false });
        }
    }
    
    [HttpGet("about")]
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
    
    [Authorize]
    [HttpGet("pricing-plan")]
    public IActionResult PricingPlan()
    {
        try
        {
            var vm = _publicHelper.GetSubscritionPlanViewModel(UserInfo);
            return View(vm);
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