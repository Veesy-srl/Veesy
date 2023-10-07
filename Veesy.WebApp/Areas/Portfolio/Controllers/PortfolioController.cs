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
            return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
        }
    }

    #region API

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

    #endregion
}