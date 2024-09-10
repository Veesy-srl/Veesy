using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Response.Subscription;
using Veesy.Service.Dtos;
using Veesy.WebApp.CustomDataAttribute;

namespace Veesy.WebApp.Api.v1;

[ApiController]
[Route("api/v1/subscription/{action}")]
[CustomAuthorize]
public class SubscriptionController : ControllerBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    private readonly ProfileHelper _profileHelper;

    public SubscriptionController(ProfileHelper profileHelper)
    {
        _profileHelper = profileHelper;
    }

    [HttpGet]
    public IActionResult List()
    {
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> ChangeUserSubscriptionPlan(string userId, string subscription)
    {
        try
        {
            var result = await _profileHelper.ChangeSubscriptionPlanApi(new ChangeSubscriptionDto
            {
                SubscriptionName = subscription,
                MyUserId = userId
            });
            
            if (result.Success)
            {
                return StatusCode(200, new SubscriptionResponse(new SubscriptionDto
                {
                    Subscription = subscription
                }));
            }
            return StatusCode(401, new SubscriptionResponse(-1, "Failed to change Subscription"));
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Error executing function ChangeUserSubscriptionPlan.");
            return StatusCode(500, new SubscriptionResponse(-1, ex.Message));
        }
    }
}