using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Veesy.Email;
using Veesy.Domain.Models;

namespace Veesy.Presentation.Helper;

public class AuthHelper
{
    private readonly UserManager<MyUser> _userManager;
    private readonly IConfiguration _config;
    private readonly IEmailSender _emailSender;

    public AuthHelper(UserManager<MyUser> userManager, IConfiguration config, IEmailSender emailSender)
    {
        _userManager = userManager;
        _config = config;
        _emailSender = emailSender;
    }

    public async Task<string> SendEmailConfirmation(string email)
    {
        var user = _userManager.FindByEmailAsync(email).Result;
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        token = HttpUtility.UrlEncode(token);
        var confirmationLink = $"{_config["ApplicationUrl"]}/Auth/ConfirmEmailVerification?token={token}&email={email}";
        var message = new Message(new (string, string)[] { ("Noreply | Swirkey", email) }, "Conferma la tua Email", confirmationLink);
        List<(string, string)> replacer = new List<(string, string)> { ("[LinkVerifyMail]", message.Content) };
        var currentPath = Directory.GetCurrentDirectory();
        await _emailSender.SendEmailAsync(message, currentPath + "/wwwroot/MailTemplate/mail-verify-email.html", replacer);
        return "Success";
    }
}