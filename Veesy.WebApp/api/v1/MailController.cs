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

    public MailController(AuthHelper authHelper)
    {
        _authHelper = authHelper;
    }
    
    [HttpGet]
    public async Task<IActionResult> SendEmail(string emailAddress, string emailType)
    {
        try
        {
            if (emailType == "welcome-email")
            {
                var result = await _authHelper.SendEmailWelcome(emailAddress);
                if (result.Success)
                {
                    return StatusCode(200, new MailResponse(new MailDto()
                    {
                        MailAddress = emailAddress
                    })); 
                }
            }
            if (emailType == "mail-verify-email")
            {
                var result = await _authHelper.SendEmailConfirmation(emailAddress);
                if (result.Success)
                {
                    return StatusCode(200, new MailResponse(new MailDto()
                    {
                        MailAddress = emailAddress
                    })); 
                }
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