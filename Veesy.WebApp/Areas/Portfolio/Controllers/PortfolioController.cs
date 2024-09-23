using System;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using NuGet.Protocol;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Model.Portfolio;
using Veesy.Service.Dtos;

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
        try
        {
            var vm = _portfolioHelper.GetListViewModel(UserInfo);
            return View(vm);
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            _notyfService.Custom("Error retrieving portfolios. Please retry.", 10 , "#ca0a0a");
            return RedirectToAction("Error400", "Public", new { area = "Public" });
        }
    }
    
    [HttpGet("portfolio/preview/{id}/{open}")]
    public IActionResult PortfolioPreview(Guid id, bool open)
    {
        try
        {
            var result = _portfolioHelper.GetPortfolioPreviewViewModel(id, open,UserInfo);
            if (!result.result.Success)
            {
                _notyfService.Custom(result.result.Message, 10 , "#ca0a0a");
                return RedirectToAction("Error400", "Public", new { area = "Public" });
            }
            return View(result.model);
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            _notyfService.Custom("Error retrieving portfolios. Please retry.", 10 , "#ca0a0a");
            return RedirectToAction("Error400", "Public", new { area = "Public" });
        }
    }
    
    [HttpGet("portfolio/settings/{id}")]
    public IActionResult Settings(Guid id)
    {
        try
        {
            var vm = _portfolioHelper.GetPortfolioSettingsViewModel(id, UserInfo);
            return View(vm);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            _notyfService.Custom("Error retrieving portfolio settings. Please retry.", 10 , "#ca0a0a");
            return RedirectToAction("Error400", "Public", new { area = "Public" });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> DeletePortfolio(PortfolioSettingsViewModel model)
    {
        try
        {
            var result = await _portfolioHelper.DeletePortfolio(model.Portfolio.Id, UserInfo);
            if (result.Success)
            {
                _notyfService.Custom("Portfolio delete correctly.", 10, "#75CCDD");
                return RedirectToAction("List", "Portfolio", new { area = "Portfolio" });
            }
            _notyfService.Custom(result.Message.Replace("'", "&#39;"), 10, "#ca0a0a");
            return RedirectToAction("Settings", "Portfolio", new { area = "Portfolio", id = model.Portfolio.Id});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            _notyfService.Custom("Error deleting file. Please retry.",10, "#ca0a0a");
            return RedirectToAction("Settings", "Portfolio", new { area = "Portfolio", id = model.Portfolio.Id});
        }
    }

    public JsonResult FakeMailSentForPortfolioPreview()
    {
        _notyfService.Custom("Email sent correctly.", 10, "#75CCDD");
        return Json(new { Result = true});
    }

    #region API

    [HttpPost]
    public async Task<JsonResult> Create([FromBody] NewPortfolioDto newPortfolioDto)
    {
        try
        {
            var response = await _portfolioHelper.CreateNewPortfolio(newPortfolioDto, UserInfo);
            if (!response.result.Success)
                _notyfService.Custom(response.result.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Portfolio create correctly.", 10, "#75CCDD");
            return Json(new { Result = response.result.Success, Message = response.result.Message, Code = response.code });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"MediaDto to update: {newPortfolioDto.ToJson()}");
            _notyfService.Custom("Error creating portfolio. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error creating portfolio. Please retry." });
        }
    }    
    
    [HttpPost]
    public async Task<JsonResult> AddMedia([FromBody] EditPortfolioDto addPortfolioDto)
    {
        try
        {
            var response = await _portfolioHelper.AddMediaToPortfolio(addPortfolioDto, UserInfo);
            if (!response.Success)
                _notyfService.Custom(response.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Portfolio update correctly.", 10, "#75CCDD");
            return Json(new { Result = response.Success, Message = response.Message, Code = addPortfolioDto.Code });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"EditPortfolioDto : {addPortfolioDto.ToJson()}");
            _notyfService.Custom("Error adding media. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error adding media. Please retry." });
        }
    }    
    
    [HttpPost]
    public async Task<JsonResult> UpdateName([FromBody] UpdatePortfolioDto portfolioDto)
    {
        try
        {
            var response = await _portfolioHelper.UpdateNamePortfolio(portfolioDto, UserInfo);
            if (!response.Success)
                _notyfService.Custom(response.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Portfolio update correctly.", 10, "#75CCDD");
            return Json(new { Result = response.Success, Message = response.Message });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"PortfolioDto to update: {portfolioDto.ToJson()}");
            _notyfService.Custom("Error updating portfolio. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating portfolio. Please retry." });
        }
    }  
    
    [HttpPost]
    public async Task<JsonResult> UpdatePassword([FromBody] UpdatePortfolioDto portfolioDto)
    {
        try
        {
            var response = await _portfolioHelper.UpdatePassword(portfolioDto, UserInfo);
            if (!response.Success)
                _notyfService.Custom(response.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Portfolio update correctly.", 10, "#75CCDD");
            return Json(new { Result = response.Success, Message = response.Message });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"PortfolioDto to update: {portfolioDto.ToJson()}");
            _notyfService.Custom("Error updating portfolio. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating portfolio. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateSecurity([FromBody] UpdatePortfolioDto portfolioDto)
    {
        try
        {
            var response = await _portfolioHelper.UpdateSecurity(portfolioDto, UserInfo);
            if (!response.Success)
                _notyfService.Custom(response.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Portfolio update correctly.", 10, "#75CCDD");
            return Json(new { Result = response.Success, Message = response.Message });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"PortfolioDto to update: {portfolioDto.ToJson()}");
            _notyfService.Custom("Error updating portfolio. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating portfolio. Please retry." });
        }
    }  
    
    [HttpPost]
    public async Task<JsonResult> SetMainPortfolio([FromBody] UpdatePortfolioDto portfolioDto)
    {
        try
        {
            var response = await _portfolioHelper.SetMainPortfolio(portfolioDto, UserInfo);
            if (!response.Success)
                _notyfService.Custom(response.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Portfolio update correctly.", 10, "#75CCDD");
            return Json(new { Result = response.Success, Message = response.Message });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"PortfolioDto to update: {portfolioDto.ToJson()}");
            _notyfService.Custom("Error updating portfolio. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating portfolio. Please retry." });
        }
    }    
    
    [HttpPost]
    public async Task<JsonResult> ChangeMediaNestedLink([FromBody] UpdateMediaNestedPortfolioDto portfolioDto)
    {
        try
        {
            var response = await _portfolioHelper.UpdateMediaLinkedPortfolio(portfolioDto, UserInfo);
            if (!response.Success)
                _notyfService.Custom(response.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Nested link update correctly.", 10, "#75CCDD");
            return Json(new { Result = response.Success, Message = response.Message });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"PortfolioDto to update: {portfolioDto.ToJson()}");
            _notyfService.Custom("Error updating nested link. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating nested link. Please retry." });
        }
    } 
    
    [HttpPost]
    public async Task<JsonResult> UpdateLayout([FromBody] UpdatePortfolioDto portfolioDto)
    {
        try
        {
            var response = await _portfolioHelper.UpdateLayout(portfolioDto, UserInfo);
            if (!response.Success)
                _notyfService.Custom(response.Message, 10, "#ca0a0a");
            return Json(new { Result = response.Success, Message = response.Message });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"PortfolioDto to update: {portfolioDto.ToJson()}");
            _notyfService.Custom("Error updating portfolio layout. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating portfolio layout. Please retry." });
        }
    }   

    [HttpPost]
    public async Task<JsonResult> Delete([FromBody] Guid portfolioId)
    {
        try
        {
            var response = await _portfolioHelper.DeletePortfolio(portfolioId, UserInfo);
            if (!response.Success)
                _notyfService.Custom(response.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Portfolio deleted correctly.", 10, "#75CCDD");
            return Json(new { Result = response.Success, Message = response.Message });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"PortfolioDto to delete: {portfolioId}");
            _notyfService.Custom("Error deleting portfolio. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating portfolio. Please retry." });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<JsonResult> DeleteByAdmin([FromBody] Guid portfolioId)
    {
        try
        {
            var response = await _portfolioHelper.DeletePortfolio(portfolioId, UserInfo);
            if (!response.Success)
                _notyfService.Custom(response.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Portfolio deleted correctly.", 10, "#75CCDD");
            return Json(new { Result = response.Success, Message = response.Message });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"PortfolioDto to delete: {portfolioId}");
            _notyfService.Custom("Error deleting portfolio. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating portfolio. Please retry." });
        }
    }
    
    [HttpPost("portfolio/publish/{portfolioId}")]
    public async Task<JsonResult> Publish([FromBody] Guid portfolioId)
    {
        try
        {
            var response = await _portfolioHelper.PublishPortfolio(portfolioId, UserInfo);
            if (!response.Success)
                _notyfService.Custom(response.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Portfolio published correctly.", 10, "#75CCDD");
            return Json(new { Result = response.Success, Message = response.Message });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"PortfolioDto to publish: {portfolioId}");
            _notyfService.Custom("Error publishing portfolio. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error publishing portfolio. Please retry." });
        }
    }

    [HttpPost]
    public async Task<JsonResult> RemoveMediaFromPortfolio([FromBody] PortfolioMediaDto portfolio)
    {
        try
        {
            var response = await _portfolioHelper.RemoveMediaFromPortfolio(portfolio, UserInfo);
            if (!response.Success)
                _notyfService.Custom(response.Message, 10, "#ca0a0a");
            else
                _notyfService.Custom("Media removed correctly.", 10, "#75CCDD");
            return Json(new { Result = response.Success, Message = response.Message });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"PortfolioDto to delete: {portfolio}");
            _notyfService.Custom("Error deleting portfolio. Please retry.", 10, "#ca0a0a");
            return Json(new { Result = false, Message = "Error updating portfolio. Please retry." });
        }
    }
    
    [HttpPost]
    public async Task<JsonResult> UpdateSortOrder([FromBody] UpdateMediaSortOrderDto dto)
    {
        try
        {
            await _portfolioHelper.UpdateSortOrder(dto, UserInfo);
            return Json(new { Result = true, Message = "Sort order updated."});
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return Json(new { Result = false, Message = "Error updating medias ordering. Please retry." });
        }
    }

    #endregion
}