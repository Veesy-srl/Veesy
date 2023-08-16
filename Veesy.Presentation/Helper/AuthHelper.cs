using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Exception;
using Veesy.Email;
using Veesy.Domain.Models;
using Veesy.Domain.Exception;
using Veesy.Presentation.Model.Auth;
using Veesy.Validators;

namespace Veesy.Presentation.Helper;

public class AuthHelper
{
    private readonly UserManager<MyUser> _userManager;
    private readonly IConfiguration _config;
    private readonly IEmailSender _emailSender;
    private readonly MyUserValidator _myUserValidator;

    public AuthHelper(UserManager<MyUser> userManager, IConfiguration config, IEmailSender emailSender, MyUserValidator myUserValidator)
    {
        _userManager = userManager;
        _config = config;
        _emailSender = emailSender;
        _myUserValidator = myUserValidator;
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

    public async Task<ResultDto> RegisterNewMember(SignUpViewModel model)
    {
        var adm = await _userManager.FindByEmailAsync(model.Email);
        if (adm != null) 
            return new ResultDto(false, "The e-mail entered has already been used.");
        
        adm = await _userManager.FindByNameAsync(model.Username);
        if (adm != null) 
            return new ResultDto(false, "The username entered has already been used.");
        
        var newUser = new MyUser()
        {
            Email = model.Email,
            UserName = model.Username,
            Name = model.Name,
            Surname = model.Surname
        };
        var validate = await _myUserValidator.UserValidator(newUser);
        if (!validate.Success) 
            return validate;
        
        IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
        if (!result.Succeeded) 
            return new ResultDto(false, result.Errors.FirstOrDefault().Description);
        
        await _userManager.AddToRolesAsync(newUser, new[] { Roles.User });
        
        return new ResultDto(true, "User create correctly.");
    }
}