using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using NLog;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Model.Admin;
using Veesy.Service.Dtos;

namespace Veesy.WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class AdminController : VeesyController
{

    private readonly AdminHelper _adminHelper;
    private readonly EndpointDataSource _endpointDataSource;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly IConfiguration _config;

    public AdminController(UserManager<MyUser> userManager, IConfiguration config, AdminHelper adminHelper, EndpointDataSource endpointDataSource) : base(userManager, config)
    {
        _config = config;
        _adminHelper = adminHelper;
        _endpointDataSource = endpointDataSource;
    }
    
    [HttpGet("overview")]
    public IActionResult Dashboard()
    {
        var vm = _adminHelper.GetDashboardViewModel();
        return View(vm);
    }
    
    [HttpGet("analytics")]
    public IActionResult Analytic()
    {
        try
        {
            var endpoints = _endpointDataSource.Endpoints;
            var routes = new List<string>(){_config["ApplicationUrl"]};
            routes.AddRange(endpoints.OfType<RouteEndpoint>()
                .Where(e => !e.RoutePattern.RawText.Contains("{") && (e.Metadata.GetMetadata<IHttpMethodMetadata>()?.HttpMethods.Contains(HttpMethods.Get) ?? false))
                .Select(e => _config["ApplicationUrl"] + "/" + e.RoutePattern.RawText)
                .ToList());
            var vm = _adminHelper.GetAnalyticViewModel(routes);
            return View(vm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet("creators-list/{page}")]
    public IActionResult CreatorsList(int page)
    {
        try
        {
            var vm = _adminHelper.GetCreatorsListViewModel(page);
            return View(vm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet("creator/{id}")]
    public IActionResult CreatorInfo(string id)
    {
        try
        {
            var vm = _adminHelper.GetCreatorViewModel(id);
            return View(vm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("subscriptions")]
    public IActionResult SubscriptionsOverview()
    {
        try
        {
            var vm = _adminHelper.GetSusbscriptionsOverviewViewModel();
            return View(vm);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            throw;
        }
    }
    
    [HttpGet("edit-subscription/{id}")]
    public IActionResult SubscriptionEdit(Guid id)
    {
        try
        {
            var vm = _adminHelper.GetSubscriptionEditViewModel(id);
            return View(vm);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            throw;
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> EditSubscription(SubscriptionEditViewModel model)
    {
        try
        {
            
            var result = await _adminHelper.EditSubscription(model.Subscription, UserInfo);
            if (!result.Success)
                throw new Exception("Failed to update subscription");
            else
                return RedirectToAction("SubscriptionsOverview");
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("match")]
    public IActionResult Match()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("creators-plus-list")]
    public IActionResult CreatorsNoFreeList()
    {
        try
        {
            var vm = _adminHelper.GetCreatorsPlusListViewModel();
            return View(vm);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            throw;
        }
    }

    public IActionResult FactoryList()
    {
        return View();
    }

    [HttpGet]
    public JsonResult GetMediaUploadedByMonth(int month)
    {
        try
        {
            var result = _adminHelper.GetMediaUploadedByMonth(month);
            var max = result.Count == 0 ? 2 : result.Max(s => s.MediaSize);
            return Json(new { Result = true, Message = "Success", MediaNumber = result.Select(s => s.NumberMedia).ToList(), MediaSize = result.Select(s => s.MediaSize).ToList(), Categories = result.Select(s => s.Day).ToList(), Max = max});
        }
        catch (Exception ex)
        {
            return Json(new { Result = false, Message = "Error"});
        }
    }
    
    [HttpGet]
    public JsonResult GetCreatorsSubscribedByMonth(int month)
    {
        try
        {
            var result = _adminHelper.GetCreatorsSubscribedByMonth(month);
            var max = result.Count == 0 ? 2 : result.Max(s => s.NumberCreator);
            return Json(new { Result = true, Message = "Success", CreatorNumber = result.Select(s => s.NumberCreator).ToList(),  Categories = result.Select(s => s.Day).ToList(), Max = max});
        }
        catch (Exception ex)
        {
            return Json(new { Result = false, Message = "Error"});
        }
    }

    [HttpGet]
    public async Task<JsonResult> ToggleReferralLink(Guid id)
    {
        try
        {
            return Json(new { Result = true, Message = "Success", Enable = await _adminHelper.ToogleReferralLink(id)});
        }
        catch (Exception ex)
        {
            return Json(new { Result = false, Message = "Error"});
        }
    }
    
    [HttpGet]
    public async Task<JsonResult> RemoveReferralLink(Guid id)
    {
        try
        {
            return Json(new { Result = await _adminHelper.DeleteReferralLink(id), Message = "Link rimosso con successo", });
        }
        catch (Exception ex)
        {
            return Json(new { Result = false, Message = "Error"});
        }
    }

    [HttpPost]
    public async Task<JsonResult> AddReferralLink([FromBody] ReferralLinkDto referralLinkDto)
    {
        try
        {
            var res = await _adminHelper.AddReferralLink(referralLinkDto);
            
            if(string.IsNullOrEmpty(res))
                return Json(new { Result = true, Message = "Link aggiunto correttamente."});
            
            return Json(new { Result = false, Message = res});
        }
        catch (Exception ex)
        {
            return Json(new { Result = false, Message = "Errore durante l'aggiunta del link"});
        }
    }
}