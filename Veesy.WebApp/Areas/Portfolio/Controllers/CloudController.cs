using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;

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
            _notyfService.Custom("Error retrieve medias. Please retry.", 10 , "#ca0a0a96");
            return RedirectToAction("Index", "Home");
        }
    }
}