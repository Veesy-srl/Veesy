using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.WebApp.Areas.Auth.Controllers;

namespace Veesy.WebApp.Areas.Account.Controllers;

[Area("Account")]
public class ProfileController : VeesyController
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    private readonly ProfileHelper _profileHelper;

    public ProfileController(ProfileHelper profileHelper, UserManager<MyUser> userManager) : base(userManager)
    {
        _profileHelper = profileHelper;
    }

    [HttpGet("profile")]
    public IActionResult Profile()
    {
        try
        {
            return View(_profileHelper.GetProfileViewModel(UserInfo));
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home", new { area = "Portfolio" });
        }
    }
    
    [HttpGet("profile/basic-info")]
    public IActionResult BasicInfo()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateBiography([FromBody] string biography)
    {
        try
        {
            var result = await _profileHelper.UpdateMyUserBio(biography, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error saving biography. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdatePortfolioIntro([FromBody] string introPortfolio)
    {
        try
        {
            var result = await _profileHelper.UpdateMyUserPortfolioIntro(introPortfolio, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error saving portfolio intro. Please retry." });
        }
    }
}