using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Presentation.Helper;
using Veesy.WebApp.CustomDataAttribute;

namespace Veesy.WebApp.Api.v1;

[ApiController]
[Route("api/v1/profile/{Action}")]
[VeesyAuthorize]
public class ProfileController : ControllerBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    private readonly ProfileHelper _profileHelper;

    public ProfileController(ProfileHelper profileHelper)
    {
        _profileHelper = profileHelper;
    }
    
    [HttpGet]
    public async Task<IActionResult> RemoveOldUser()
    {
        try
        {
            Logger.Info("RemoveOldUser function started...");
            await _profileHelper.RemoveOldUser();
            Logger.Info("RemoveOldUser function done.");
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Error executing function RemoveOldUser.");
            return BadRequest();
        }
    }
}