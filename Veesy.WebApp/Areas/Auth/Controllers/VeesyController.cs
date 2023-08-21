using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Veesy.Domain.Models;

namespace Veesy.WebApp.Areas.Auth.Controllers;

public class VeesyController : Controller
{
    public MyUser UserInfo;
    private readonly UserManager<MyUser> _userManager;

    public VeesyController(UserManager<MyUser> userManager)
    {
        _userManager = userManager;
    }
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        UserInfo = _userManager.GetUserAsync(this.User).GetAwaiter().GetResult();
        base.OnActionExecuting(context);
    }
}