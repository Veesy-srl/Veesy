using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using NuGet.Protocol;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Email;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Model.Auth;

namespace Veesy.WebApp.Areas.Auth.Controllers;

[Area("Auth")]
public class AuthController : Controller
{
    private readonly AuthHelper _authHelper;
    private readonly INotyfService _notyfService;
    private readonly SignInManager<MyUser> _signInManager;
    private readonly UserManager<MyUser> _userManager;
    private readonly IConfiguration _config;
    private readonly IEmailSender _emailSender;
    
    
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public AuthController(AuthHelper authHelper, SignInManager<MyUser> signInManager, UserManager<MyUser> userManager, IConfiguration config, IEmailSender emailSender, INotyfService notyfService)
    {
        _authHelper = authHelper;
        _signInManager = signInManager;
        _userManager = userManager;
        _config = config;
        _emailSender = emailSender;
        _notyfService = notyfService;
    }

    [HttpGet("auth/login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost("auth/login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        try
        {
            if(string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                _notyfService.Custom("Email or password are invalid", 10, "#ca0a0a");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    _notyfService.Custom("Email or password are invalid", 10, "#ca0a0a");
                    return View(model);
                }
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return RedirectToAction("SendEmailVerification", new { email = user.Email });
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                //_authHelper.AddLastLogin(user);
                if((await _userManager.GetRolesAsync(user))[0] == Roles.Admin)
                    return RedirectToAction("Dashboard", "Admin", new { area = "Admin" });
                return RedirectToAction("Index", "Home", new { area = "Portfolio" });
            }
            _notyfService.Custom("Email or password are invalid", 10, "#ca0a0a");
            return View(model);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            return View(model);
        }

    }
    
    
    [HttpGet]
    public IActionResult SignUp()
    {
        var vm = _authHelper.GetSignUpViewModel();
        return View(vm);
    }
    
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        try
        {
            var result = await _authHelper.RegisterNewMember(model);
            if (result.Success)
                return RedirectToAction("SendEmailVerification", new {email = model.Email});
            _notyfService.Custom(result.Message.Replace("'", "&#39;"), 10, "#ca0a0a");
            var vm = _authHelper.GetSignUpViewModelException(model);
            return View(vm);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
            Logger.Error($"Signup model: {model.ToJson()}");
            _notyfService.Custom("Error during register new member. Please retry.", 10, "#ca0a0a");
            var vm = _authHelper.GetSignUpViewModelException(model);
            return View();
        }
    }
    
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(LoginViewModel model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                    return RedirectToAction("ForgotPasswordComplete", "Auth");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = HttpUtility.UrlEncode(token);
            var resetLink = $"{_config["ApplicationUrl"]}/Auth/Auth/ResetPassword?token={token}&email={model.Email}";
            var message = new Message(new (string, string)[] { ("Noreply | Veesy", user.Email) }, "Reset your password", resetLink);
            List<(string, string)> replacer = new List<(string, string)> { ("[LinkResetPassword]", resetLink) };
            var currentPath = Directory.GetCurrentDirectory();
            await _emailSender.SendEmailAsync(message, currentPath + "/wwwroot/MailTemplate/mail-reset-password.html", replacer);
            return RedirectToAction("ForgotPasswordComplete", "Auth");
        }
        catch (Exception e)
        {
            _notyfService.Custom("Error during send reset password link. Please retry.", 10, "#ca0a0a");
            Logger.Error(e, e.Message);
            Logger.Error($"Forgot password: {model.ToJson()}");
            return View();
        }
    }
    
    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        var vm = new ResetPasswordViewModel()
        {
            Token = token,
            Email = email
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(model.Email);
                if(user == null)
                    return RedirectToAction("Login", "Auth");
            }

            if(model.Password != model.PasswordConfirm)
            {
                _notyfService.Custom("Passwords don't match", 10, "#ca0a0a");
                return View(model);
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                _notyfService.Custom("Password update correctly.", 10, "#75CCDD");
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                _notyfService.Custom(result.Errors.FirstOrDefault().Description, 10, "#ca0a0a");
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _notyfService.Custom("Error during updating password. Please retry.", 10, "#ca0a0a");
            Logger.Error(ex, ex.Message);
            Logger.Error($"Reset password: {model.ToJson()}");
            return View(model);
        }
    }
    
    [HttpGet] 
    public async Task<IActionResult> ConfirmEmailVerification(string token, string email)
    {
        try
        {
            if(email == "info@veesy.eu")
                return RedirectToAction("SignUp", "Auth");
            
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(email);
                if(user == null)
                    return RedirectToAction("SignUp", "Auth");
            }

            if ((await _userManager.ConfirmEmailAsync(user, token)).Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _authHelper.SendEmailWelcome(email);
                return RedirectToAction("EmailVerified", "Auth", new {area = "Auth"});
            }
            
            return RedirectToAction("VerifyEmail", "Auth", new { Email = email });
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            _notyfService.Custom("Error send email confirmation. Please retry.", 10, "#ca0a0a");
            return RedirectToAction("Login", "Auth");
        }
    }

    [HttpGet]
    public IActionResult VerifyEmail(string email)
    {
        var vm = new VerifyEmailViewModel()
        {
            Email = email
        };
        return View(vm);
    }
    
    [HttpGet]
    public IActionResult EmailVerified()
    {
        return View();
    }
        
    [HttpGet]
    public async Task<IActionResult> SendEmailVerification(string email)
    {
        try
        {
            await _authHelper.SendEmailConfirmation(email);
            return RedirectToAction("VerifyEmail", new { email = email });
        }
        catch (Exception e)
        {
            Logger.Error(e, e.Message);
            _notyfService.Custom("Error send email verification. Please retry.", 10, "#ca0a0a");
            return RedirectToAction("VerifyEmail", new { email = email });
        }
    }

    [HttpGet("auth/logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Auth");
    }

    [HttpGet]
    public IActionResult ForgotPasswordComplete()
    {
        return View();
    }
}