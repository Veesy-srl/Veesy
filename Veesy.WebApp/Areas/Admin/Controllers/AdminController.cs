using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;

namespace Veesy.WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class AdminController : VeesyController
{

    private readonly AdminHelper _adminHelper;
    public AdminController(UserManager<MyUser> userManager, IConfiguration config, AdminHelper adminHelper) : base(userManager, config)
    {
        _adminHelper = adminHelper;
    }
    
    [HttpGet]
    public IActionResult Dashboard()
    {
        var vm = _adminHelper.GetDashboardViewModel();
        return View(vm);
    }
    
    [HttpGet("freelancers")]
    public IActionResult FreelancersList()
    {
        try
        {
            var vm = _adminHelper.GetFreelancersListViewModel();
            return View(vm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet("freelancer/{id}")]
    public IActionResult FreelancerInfo(string id)
    {
        try
        {
            var vm = _adminHelper.GetFreelancerViewModel(id);
            return View(vm);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}