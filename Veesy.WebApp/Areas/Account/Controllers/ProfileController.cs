using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Service.Dtos;
using Veesy.WebApp.Areas.Auth.Controllers;

namespace Veesy.WebApp.Areas.Account.Controllers;

[Authorize]
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
        try
        {
            return View(_profileHelper.GetBasicInfoViewModel(UserInfo));
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Profile");
        }
    }

    #region API
    
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
    
    [HttpPost]
    public async Task<JsonResult> UpdateExternalLink([FromBody] string externalLink)
    {
        try
        {
            var result = await _profileHelper.UpdateMyUserExternalLink(externalLink, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error saving portfolio intro. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateUsedSoftware([FromBody] List<Guid> usedSoftwareCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateUsedSoftware(usedSoftwareCodes, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating softwares. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateHardSkills([FromBody] List<Guid> hardSkillsCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateSkill(hardSkillsCodes, UserInfo, SkillConstants.HardSkill);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating softwares. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateCategoriesWork([FromBody] List<Guid> categoriesWorkCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateCategoriesWork(categoriesWorkCodes, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating roles. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateLanguageSpoken([FromBody] List<Guid> languagesSpokenCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateLanguageSpoken(languagesSpokenCodes, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating languages. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateInfoToShow([FromBody] List<Guid> infoToShowCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateInfoToShow(infoToShowCodes, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating languages. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateNameAndSurname([FromBody] string name, string surname)
    {
        try
        {
            var result = await _profileHelper.UpdateFullName(name, surname, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating languages. Please retry." });
        }
    }
    
    #endregion
}