using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Model.Cloud;

namespace Veesy.WebApp.Areas.Public.Controllers;

[Area("Public")]
public class PublicController : VeesyController
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly PublicHelper _publicHelper;
    private readonly IConfiguration _config;


    public PublicController(UserManager<MyUser> userManager, PublicHelper publicHelper, IConfiguration config) : base(
        userManager, config)
    {
        _publicHelper = publicHelper;
        _config = config;
    }
    
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
    
    [HttpGet("Splash")]
    public IActionResult Splash()
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
    
    [HttpGet("Creators")]
    public IActionResult Creators()
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
    
    [HttpGet("About")]
    public IActionResult About()
    {
        try
        {
            AboutMediaViewModel about = new AboutMediaViewModel();
            var mediaList = _publicHelper.GetListMedia(7);
            about.BasePath = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/";
            about.BasePathImages = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/";
            about.MediaList = mediaList.Select(item => item.ImgPath).ToList();
            about.MediaUser = mediaList.Select(item => item.userImg).ToList();
            about.Usernames = mediaList.Select(item => item.Username).ToList();

            return View(about);
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
}