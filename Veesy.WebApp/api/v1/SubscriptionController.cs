using Microsoft.AspNetCore.Mvc;
using Veesy.WebApp.CustomDataAttribute;

namespace Veesy.WebApp.Api.v1;

[ApiController]
[Route("api/v1/subscription/{action}")]
[CustomAuthorize]
public class SubscriptionController : ControllerBase
{
    [HttpGet]
    public IActionResult List()
    {
        return Ok();
    }
}