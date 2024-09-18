using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Veesy.Domain.Exceptions;
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
    
    [HttpGet("contact")]
    public IActionResult Contact()
    {
        try 
        {
            return View();
        }
        catch (Exception e) 
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Error400");
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
            return RedirectToAction("Error400");
        }
    }
    
    [HttpGet("privacy-policy")]
    public IActionResult PrivacyPolicy()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Error400");
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
            var vm = _publicHelper.GetSplashViewModel();
            return View(vm);
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Error400");
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
            return RedirectToAction("Error400");
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
            return RedirectToAction("Error400");
        }
    } 
    
    [HttpGet("gallery")]
    public IActionResult Gallery()
    {
        try
        {
            var vm = _publicHelper.GetGalleryViewModel();
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
    public async Task<IActionResult> About()
    {
        try
        {
            var vm = await _publicHelper.GetAboutInfo();
            return View(vm);
        }
        catch (Exception e)
        { 
            Logger.Error(e, e.Message);
            return RedirectToAction("Error400");
        }
    }
    
     
    [HttpGet("landing")]
    public async Task<IActionResult> Landing()
    {
        try
        {
            var vm = await _publicHelper.GetAboutInfo();
            return View(vm);
        }
        catch (Exception e)
        { 
            Logger.Error(e, e.Message);
            return RedirectToAction("Error400");
        }
    }
    
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
            return RedirectToAction("Error400");
        }
    }
    
    [HttpGet("terms-and-conditions")]
    public IActionResult TermsAndConditions()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Error400");
        }
    }
    
    
    [HttpGet("portfolio/{id}")]
    public IActionResult Portfolio(Guid id)
    {
        try
        {
            var res = _portfolioHelper.GetPortfolioById(id, null, null);
            if (!res.resultDto.Success)
            {
                _notyfService.Custom(res.resultDto.Message, 10, "#ca0a0a");
                return RedirectToAction("Error400");
            }

            return RedirectToAction("Portfolio", new {user = res.userFullname, portfolioname = res.portfolioName});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return RedirectToAction("Error400");
        }
    }

    [HttpGet("folio/{user}/{portfolioname}")]
    public IActionResult Portfolio(string user, string portfolioname)
    {
        try
        {
            var res = _portfolioHelper.GetPortfolioViewModel(Guid.Empty, user, portfolioname);
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
    
    [HttpPost("folio/{user}/{portfolioname}")]
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

    [HttpPost]
    public async Task<JsonResult> SendEmailToCreator([FromBody] CreatorFormDto form)
    {
        try
        {
            var result = await _publicHelper.SendCreatorForm(form, new MyUser
            {
                Id = Guid.Empty.ToString()
            }, null);

            if (!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Email send correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message });
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            _notyfService.Custom(e.Message, 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error during send mail." });
        }
    }
    
    
    
    [HttpGet("400")]
    public IActionResult Error400()
    {
        return View();
    }
    
    [HttpGet("404")]
    public IActionResult Error404()
    {
        return View();
    }
}