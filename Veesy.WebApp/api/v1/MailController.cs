using Microsoft.AspNetCore.Mvc;
using NLog;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Email;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Response;
using Veesy.Presentation.Response.Mail;
using Veesy.Service.Dtos;
using Veesy.WebApp.CustomDataAttribute;

namespace Veesy.WebApp.Api.v1;

[ApiController]
[Route("api/v1/mail/{action}")]
[CustomAuthorize]
public class MailController : ControllerBase
{
    private readonly AuthHelper _authHelper;
    private readonly IEmailSender _emailSender;
    private readonly ProfileHelper _profileHelper;
    private readonly PublicHelper _publicHelper;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public MailController(AuthHelper authHelper, IEmailSender emailSender, ProfileHelper profileHelper, PublicHelper publicHelper)
    {
        _authHelper = authHelper;
        _emailSender = emailSender;
        _profileHelper = profileHelper;
        _publicHelper = publicHelper;
    }
    
    [HttpGet]
    public async Task<IActionResult> SendEmail(string email, string name)
    {
        try
        {
            name = name.ToLower();
            if (name == "welcome-email")
            {
                var result = await _authHelper.SendEmailWelcome(email);
                if (result.Success)
                {
                    return StatusCode(200, new MailResponse(new MailDto()
                    {
                        MailAddress = email
                    })); 
                }
                return StatusCode(400, new MailResponse(-1, result.Message));
            }
            else if (name == "mail-reset-password")
            {
                var result = await _authHelper.SendEmailResetPassword(email);
                if (result.Success)
                {
                    return StatusCode(200, new MailResponse(new MailDto()
                    {
                        MailAddress = email
                    })); 
                }

                return StatusCode(400, new MailResponse(-1, result.Message));
            }
            else if (name == "mail-verify-email")
            {
                var result = await _authHelper.SendEmailConfirmation(email);
                if (result.Success)
                {
                    return StatusCode(200, new MailResponse(new MailDto()
                    {
                        MailAddress = email
                    })); 
                }
                return StatusCode(400, new MailResponse(-1, result.Message));
            }
            else if (name == "mail-update-pro")
            {
                var result = await _profileHelper.SendEmailProPlan(email);
                if (result.Success)
                {
                    return StatusCode(200, new MailResponse(new MailDto()
                    {
                        MailAddress = email
                    })); 
                }
                return StatusCode(400, new MailResponse(-1, result.Message));
            }
            else if (name == "mail-creator-form")
            {
                var result = await _publicHelper.SendCreatorForm(new CreatorFormDto
                {
                    SenderEmail = "noreply@veesy.eu",
                    SenderName = "Veesy",
                    Message = "Questo è un messaggio di prova."
                }, new MyUser
                {
                    Id = Guid.Empty.ToString()
                }, email);
                if (result.Success)
                {
                    return StatusCode(200, new MailResponse(new MailDto()
                    {
                        MailAddress = email
                    })); 
                }
                return StatusCode(400, new MailResponse(-1, result.Message));
            }
            return StatusCode(401, new MailResponse(-1, "Wrong mail type"));
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            return StatusCode(500, new MailResponse(-1, "Error sending email."));
        }
    }
    
    
    [HttpGet]
    public async Task<IActionResult> EmailCreator(string emailAddress, string emailMessage)
    {
        try
        {
            if (emailMessage != null)
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
            return StatusCode(401, new MailResponse(-1, "Insert message"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}