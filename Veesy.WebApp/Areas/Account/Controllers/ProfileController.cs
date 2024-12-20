using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using NuGet.Protocol;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Service.Dtos;

namespace Veesy.WebApp.Areas.Account.Controllers;

[Area("Account")]
public class ProfileController : VeesyController
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    private readonly ProfileHelper _profileHelper;
    private readonly INotyfService _notyfService;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<MyUser> _signInManager;

    public ProfileController(ProfileHelper profileHelper, UserManager<MyUser> userManager, INotyfService notyfService, IConfiguration configuration, SignInManager<MyUser> signInManager) : base(userManager, configuration)
    {
        _profileHelper = profileHelper;
        _notyfService = notyfService;
        _configuration = configuration;
        _signInManager = signInManager;
    }

    [Authorize]
    [HttpGet("profile")]
    public IActionResult Profile()
    {
        try
        {
            var vm = _profileHelper.GetProfileViewModel(UserInfo);
            return View(vm);
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home", new { area = "Portfolio" });
        }
    }
    
    [Authorize]
    [HttpGet("profile/basic-info")]
    public IActionResult BasicInfo()
    {
        try
        {
            var vm = _profileHelper.GetBasicInfoViewModel(UserInfo);
            return View(vm);
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Profile");
        }
    }

    #region API
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateBiography([FromBody] string biography)
    {
        try
        {
            var result = await _profileHelper.UpdateMyUserBio(biography, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Biography update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Biografia: {biography}");
            _notyfService.Custom("Error updating biography. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error saving biography. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> DeleteAccount([FromBody] string id)
    {
        try
        {
            var result = await _profileHelper.DeleteAccount(id, true);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom(result.Message, 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            _notyfService.Custom("Error deleting account. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error deleting account. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> DeleteAccountByUser([FromBody] string id)
    {
        try
        {
            var result = await _profileHelper.DeleteAccount(id,false);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom(result.Message, 10, "#75CCDD");
            await _signInManager.SignOutAsync();
            return Json(new { Result = true, RedirectUrl = Url.Action("Splash", "Public", new { area = "Public" }) });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            _notyfService.Custom("Error deleting account. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error deleting account. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> ChangeSubscriptionPlan([FromBody] ChangeSubscriptionDto changeSubscriptionDto)
    {
        try
        {
            var result = await _profileHelper.ChangeSubscriptionPlan(changeSubscriptionDto, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom(result.Message, 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            _notyfService.Custom("Error updating subscription plan. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error saving subscription plan. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateUserVisibility([FromBody] UpdateUserVisibilityDto updateUserVisibility)
    {
        try
        {
            var result = await _profileHelper.UpdateUserVisibility(updateUserVisibility, UserInfo);
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating user visibility. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdatePortfolioIntro([FromBody] string introPortfolio)
    {
        try
        {
            var result = await _profileHelper.UpdateMyUserPortfolioIntro(introPortfolio, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Portfolio update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Intor portfolio: {introPortfolio}");
            _notyfService.Custom("Error updating portfolio intro. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error saving portfolio intro. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateExternalLink([FromBody] string externalLink)
    {
        try
        {
            var result = await _profileHelper.UpdateMyUserExternalLink(externalLink, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("External link update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Link esterno: {externalLink}");
            _notyfService.Custom("Error updating external link. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error saving external link. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateUsedSoftware([FromBody] List<Guid> usedSoftwareCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateUsedSoftware(usedSoftwareCodes, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Softwares update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Software id: {usedSoftwareCodes.ToJson()}");
            _notyfService.Custom("Error updating softwares. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating softwares. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateHardSkills([FromBody] List<Guid> hardSkillsCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateSkill(hardSkillsCodes, UserInfo, SkillConstants.HardSkill);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Hard skills update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Hard skill id: {hardSkillsCodes.ToJson()}");
            _notyfService.Custom("Error updating hard skills. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating hard skills. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateSoftSkills([FromBody] List<Guid> softSkillsCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateSkill(softSkillsCodes, UserInfo, SkillConstants.SoftSkill);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Soft skills update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Soft skill id: {softSkillsCodes.ToJson()}");
            _notyfService.Custom("Error updating soft skills. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating soft skills. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateCategoriesWork([FromBody] List<Guid> categoriesWorkCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateCategoriesWork(categoriesWorkCodes, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Categories update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Categories work id: {categoriesWorkCodes.ToJson()}");
            _notyfService.Custom("Error updating categories. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating roles. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateLanguageSpoken([FromBody] List<Guid> languagesSpokenCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateLanguageSpoken(languagesSpokenCodes, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Languages update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Languages spoken id: {languagesSpokenCodes.ToJson()}");
            _notyfService.Custom("Error updating languages. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating languages. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateInfoToShow([FromBody] List<Guid> infoToShowCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateInfoToShow(infoToShowCodes, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else _notyfService.Custom("Info to show update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Info to show id: {infoToShowCodes.ToJson()}");
            _notyfService.Custom("Error updating info. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating info to show. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateNameAndSurname([FromBody] FullNameDto fullName)
    {
        try
        {
            var result = await _profileHelper.UpdateFullName(fullName.Name, fullName.Surname, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Fullname update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Fullname: {fullName.Name} - {fullName.Surname}");
            _notyfService.Custom("Error updating fullname. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating fullname. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateEmail([FromBody] string email)
    {
        try
        {
            var result = await _profileHelper.UpdateEmail(email, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Email update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Email: {email}");
            _notyfService.Custom("Error updating email. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating email. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateUsername([FromBody] string username)
    {
        try
        {
            var result = await _profileHelper.UpdateUsername(username, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Username update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Username: {username}");
            _notyfService.Custom("Error updating username. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating username. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdatePassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var result = await _profileHelper.UpdatePassword(resetPasswordDto.OldPassword, resetPasswordDto.NewPassword, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Password update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Reset password dto: {resetPasswordDto.ToJson()}");
            _notyfService.Custom("Error updating password. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating password. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdatePhoneNumber([FromBody] PhoneNumberDto phoneNumberDto)
    {
        try
        {
            var result = await _profileHelper.UpdatePhoneNumber(phoneNumberDto.Prefix, phoneNumberDto.PhoneNumber, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Phone number update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Phone Number: {phoneNumberDto.Prefix} {phoneNumberDto.PhoneNumber}");
            _notyfService.Custom("Error updating phone number. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating phone number. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateVATNumber([FromBody] string vatNumber)
    {
        try
        {
            var result = await _profileHelper.UpdateVATNumber(vatNumber, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("VAT number update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"VAT: {vatNumber}");
            _notyfService.Custom("Error updating VAT number. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating VAT number. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateCategory([FromBody] string category)
    {
        try
        {
            var result = await _profileHelper.UpdateCategory(category, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Role update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Category: {category}");
            _notyfService.Custom("Error updating category. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating category. Please retry." });
        }
    }
    
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateSectors([FromBody] List<Guid> sectorCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateSectors(sectorCodes, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Fields update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Sectors id: {sectorCodes.ToJson()}");
            _notyfService.Custom("Error updating sectors. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating sectors. Please retry." });
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> UpdateRolesWork([FromBody] List<Guid> roleCodes)
    {
        try
        {
            var result = await _profileHelper.UpdateRolesWorks(roleCodes, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Roles update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Roles id: {roleCodes.ToJson()}");
            _notyfService.Custom("Error updating sectors. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating roles. Please retry." });
        }
    }
    
    
    [Authorize]
    [HttpGet("callbackdiscord")]
    public async Task<IActionResult> CallbackDiscord(string code)
    {
        try
        {
            await _profileHelper.ConnectDiscord(code, UserInfo);
            return RedirectToAction("Index", "Home", new {area = "Portfolio"});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
			      return RedirectToAction("Index", "Home", new { area = "Portfolio" });
			      /*return RedirectToAction("Error400", "Public", new {area = "Public"});*/
        }
    }
    
    [HttpGet("profile/unsubscribe/{userId}")]
    public async Task<IActionResult> UnsubscribeMail(string userId)
    {
        try
        {
            await _profileHelper.UnsubscribeMail(userId);
            return RedirectToAction("Unsubscribed");
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return RedirectToAction("Error400", "Public", new {area = "Public"});
        }
    }

    public IActionResult Unsubscribed()
    {
        return View();
    }
    
    #endregion

}