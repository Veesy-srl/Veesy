using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NuGet.Protocol;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Service.Dtos;

namespace Veesy.WebApp.Areas.Public.Controllers;

[Area("Public")]
public class PublicController : VeesyController
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public PublicController(UserManager<MyUser> userManager, IConfiguration config) : base(userManager, config) {}
    
    [HttpGet("Contacts")]
    public IActionResult Contacts()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
}