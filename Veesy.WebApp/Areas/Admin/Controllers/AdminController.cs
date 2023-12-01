using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;

namespace Veesy.WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class AdminController : VeesyController
{

    private readonly AdminHelper _adminHelper;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public AdminController(UserManager<MyUser> userManager, IConfiguration config, AdminHelper adminHelper) : base(userManager, config)
    {
        _adminHelper = adminHelper;
    }
    
    [HttpGet("overview")]
    public IActionResult Dashboard()
    {
        var vm = _adminHelper.GetDashboardViewModel();
        return View(vm);
    }
    
    [HttpGet("creators-list")]
    public IActionResult CreatorsList()
    {
        try
        {
            var vm = _adminHelper.GetCreatorsListViewModel();
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
    public IActionResult ManageSubscriptions()
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
}