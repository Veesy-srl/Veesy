using Microsoft.AspNetCore.Mvc;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Response.Mail;
using Veesy.WebApp.CustomDataAttribute;

namespace Veesy.WebApp.Api.v1;

[ApiController]
[Route("api/v1/mail/{action}")]
[CustomAuthorize]
public class MailController : ControllerBase
{
    private readonly AuthHelper _authHelper;
    private readonly ProfileHelper _profileHelper;

    public MailController(AuthHelper authHelper, ProfileHelper profileHelper)
    {
        _authHelper = authHelper;
        _profileHelper = profileHelper;
    }
    
    [HttpGet]
    public async Task<IActionResult> SendEmail(string address, string name)
    {
        try
        {
            name = name.ToLower();
            if (name == "welcome-email")
            {
                var result = await _authHelper.SendEmailWelcome(address);
                if (result.Success)
                {
                    return StatusCode(200, new MailResponse(new MailDto()
                    {
                        MailAddress = address
                    })); 
                }
            }
            else if (name == "mail-verify-email")
            {
                var result = await _authHelper.SendEmailConfirmation(address);
                if (result.Success)
                {
                    return StatusCode(200, new MailResponse(new MailDto()
                    {
                        MailAddress = address
                    })); 
                }
            }
            else if (name == "mail-update-pro")
            {
                var result = await _profileHelper.SendEmailProPlan(address);
                if (result.Success)
                {
                    return StatusCode(200, new MailResponse(new MailDto()
                    {
                        MailAddress = address
                    })); 
                }
                return StatusCode(401, new MailResponse(-1, result.Message));
            }
            return StatusCode(401, new MailResponse(-1, "Wrong mail type"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}