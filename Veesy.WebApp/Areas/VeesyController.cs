using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;

namespace Veesy.WebApp.Areas;

public class VeesyController : Controller
{
    public MyUser UserInfo;
    private readonly UserManager<MyUser> _userManager;
    private readonly IConfiguration _config;

    public VeesyController(UserManager<MyUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }
    
    public override async void OnActionExecuting(ActionExecutingContext context)
    {
        UserInfo = _userManager.GetUserAsync(this.User).GetAwaiter().GetResult();
        ViewBag.BaseDirectory = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/";
        ViewBag.ProfileImage = "";
        if (UserInfo != null)
            ViewBag.ProfileImage = UserInfo.ProfileImageFileName;
        base.OnActionExecuting(context);
    }
}