using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Service.Dtos;
using Veesy.WebApp.Areas.Auth.Controllers;
using Veesy.WebApp.CustomDataAttribute;

namespace Veesy.WebApp.Areas.Account.Controllers;

[Authorize]
[Area("Account")]
public class ProfileController : VeesyController
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    private readonly ProfileHelper _profileHelper;
    private readonly INotyfService _notyfService;

    public ProfileController(ProfileHelper profileHelper, UserManager<MyUser> userManager, INotyfService notyfService) : base(userManager)
    {
        _profileHelper = profileHelper;
        _notyfService = notyfService;
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
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a96");
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
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a96");
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
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a96");
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
    public async Task<JsonResult> UpdateSoftSkills([FromBody] List<Guid> softSkillsCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateSkill(softSkillsCodes, UserInfo, SkillConstants.SoftSkill);
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
    public async Task<JsonResult> UpdateNameAndSurname([FromBody] FullNameDto fullName)
    {
        try
        {
            var result = await _profileHelper.UpdateFullName(fullName.Name, fullName.Surname, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating fullname. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateEmail([FromBody] string email)
    {
        try
        {
            var result = await _profileHelper.UpdateEmail(email, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating email. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateUsername([FromBody] string username)
    {
        try
        {
            var result = await _profileHelper.UpdateUsername(username, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating username. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdatePassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var result = await _profileHelper.UpdatePassword(resetPasswordDto.OldPassword, resetPasswordDto.NewPassword, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating password. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdatePhoneNumber([FromBody] string phoneNumber)
    {
        try
        {
            var result = await _profileHelper.UpdatePhoneNumber(phoneNumber, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating phone number. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateVATNumber([FromBody] string vatNumber)
    {
        try
        {
            var result = await _profileHelper.UpdateVATNumber(vatNumber, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating VAT number. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateCategory([FromBody] string category)
    {
        try
        {
            var result = await _profileHelper.UpdateCategory(category, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating category. Please retry." });
        }
    }
    
    #endregion
}