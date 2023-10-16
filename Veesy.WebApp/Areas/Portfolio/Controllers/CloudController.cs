using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NuGet.Protocol;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Service.Dtos;

namespace Veesy.WebApp.Areas.Portfolio.Controllers;

[Authorize]
[Area("Portfolio")]
public class CloudController : VeesyController
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    private readonly INotyfService _notyfService;
    private readonly CloudHelper _cloudHelper;

    public CloudController(CloudHelper cloudHelper, INotyfService notyfService, UserManager<MyUser> userManager, IConfiguration configuration) : base(userManager, configuration)
    {
        _cloudHelper = cloudHelper;
        _notyfService = notyfService;
    }

    [HttpGet("cloud")]
    public IActionResult List()
    {
        try
        {
            var vm = _cloudHelper.GetCloudViewModel(UserInfo);
            return View(vm);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            _notyfService.Custom("Error retrieving medias. Please retry.", 10 , "#ca0a0a");
            return RedirectToAction("Index", "Home");
        }
    }
    
    [HttpGet("cloud/edit/{id}")]
    public IActionResult Edit(Guid id)
    {
        try
        {
            var response = _cloudHelper.GetEditViewModel(id, UserInfo);
            if (!response.resultDto.Success)
            {
                _notyfService.Custom(response.resultDto.Message, 10 , "#ca0a0a");
                return RedirectToAction("List");    
            }
            return View(response.viewModel);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return RedirectToAction("List");
        }
    }
    
    [HttpGet("cloud/{id}")]
    public IActionResult SingleMedia(Guid id)
    {
        try
        {
            var response = _cloudHelper.GetSingleMediaViewModel(id, UserInfo);
            if (!response.resultDto.Success)
            {
                _notyfService.Custom(response.resultDto.Message, 10 , "#ca0a0a");
                return RedirectToAction("Edit", new {id = id});    
            }
            return View(response.viewModel);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            _notyfService.Custom("Error server repsonse. Please retry.", 10 , "#ca0a0a");
            return RedirectToAction("Edit", new {id = id});
        }
    }

    #region API

    [HttpPost]
    public async Task<JsonResult> UpdateFileName([FromBody] MediaDto media)
    {
        try
        {
            var result = await _cloudHelper.UpdateFileName(media, UserInfo);
            if(!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else 
                _notyfService.Custom("Filename update correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"MediaDto to update: {media.ToJson()}");
            _notyfService.Custom("Error updating Filename. Please retry.", 10 , "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating Filename. Please retry." });
        }
    }

    public async Task<JsonResult> UpdateCredits([FromBody] MediaDto media)
    {
        try
        {
            var result = await _cloudHelper.UpdateFileUpdateCredits(media, UserInfo);
            if (!result.Success)
                _notyfService.Custom(result.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Credits updated correctly.", 10, "#75CCDD");
            return Json(new { Result = result.Success, Message = result.Message });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"MediaDto to update: {media.ToJson()}");
            _notyfService.Custom("Error updating Filename. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating Filename. Please retry." });
        }
    }

    #endregion

}