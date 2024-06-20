using System.Net.Mail;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NLog;
using Veesy.Domain.Constants;
using Veesy.Domain.Exceptions;
using Veesy.Email;
using Veesy.Domain.Models;
using Veesy.Presentation.Model.Auth;
using Veesy.Service.Interfaces;
using Veesy.Validators;

namespace Veesy.Presentation.Helper;

public class AuthHelper
{
    private readonly UserManager<MyUser> _userManager;
    private readonly IConfiguration _config;
    private readonly IEmailSender _emailSender;
    private readonly MyUserValidator _myUserValidator;
    private readonly IAccountService _accountService;
    private readonly ISubscriptionPlanService _subscriptionPlanService;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private static readonly Random random = new Random();

    public AuthHelper(UserManager<MyUser> userManager, IConfiguration config, IEmailSender emailSender, MyUserValidator myUserValidator, IAccountService accountService, ISubscriptionPlanService subscriptionPlanService)
    {
        _userManager = userManager;
        _config = config;
        _emailSender = emailSender;
        _myUserValidator = myUserValidator;
        _accountService = accountService;
        _subscriptionPlanService = subscriptionPlanService;
    }

    public async Task<ResultDto> SendEmailConfirmation(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = await _userManager.FindByNameAsync(email);
            if (user == null)
                return new ResultDto(false, "User not found");
        }
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        token = HttpUtility.UrlEncode(token);
        var confirmationLink = $"{_config["ApplicationUrl"]}/Auth/Auth/ConfirmEmailVerification?token={token}&email={email}";
        var message = new Message(new (string, string)[] { ("Noreply | Veesy", email) }, "Confirm your account", confirmationLink);
        List<(string, string)> replacer = new List<(string, string)> { ("[LinkVerifyMail]", message.Content) };
        var currentPath = Directory.GetCurrentDirectory();
        await _emailSender.SendEmailAsync(message, currentPath + "/wwwroot/MailTemplate/mail-verify-email.html", replacer);
        return new ResultDto(true, "");
    }
    
    public async Task<ResultDto> SendEmailWelcome(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = await _userManager.FindByNameAsync(email);
            if (user == null)
                return new ResultDto(false, "User not found");
        }
        var name = user.Fullname;
        var recipients = new (string, string)[] { ("Noreply | Veesy", email) };
        var link = "";
        var message = new Message(new (string, string)[] { ("Noreply | Veesy", email) }, "Welcome to Veesy", link);
        List<(string, string)> replacer = new List<(string, string)> { ("[name]", name) };
        var currentPath = Directory.GetCurrentDirectory();
        
        var imageFiles = new List<string> { "Welcome/logo_welcome.png", "Welcome/mail-bottom_welcome.png", "Welcome/mail-top_welcome.png", "Welcome/social-facebook_welcome.png", "Welcome/social-instagram_welcome.png", "Welcome/social-linkedin_welcome.png", "Welcome/Welcome_welcome.gif"};
        
        await _emailSender.SendEmailAsync(message, currentPath + "/wwwroot/MailTemplate/welcome-email.html", replacer, imageFiles);
        return new ResultDto(true, "");
    }

    public async Task<ResultDto> RegisterNewMember(SignUpViewModel model)
    {
        if(model.Password == null || model.ConfirmPassword == null)
            return new ResultDto(false, "Please insert password.");
        if (model.Password != model.ConfirmPassword) 
            return new ResultDto(false, "Entered passwords do not match.");
        if(model.SelectedCategoriesWork == null || model.SelectedCategoriesWork.Count < 1 || model.SelectedCategoriesWork.Count > 3)
            return new ResultDto(false, "Select at least one category and not more than three.");
        var imageDefault = SelectRandomImageName();
        var now = DateTime.Now;
        var categories = new List<MyUserCategoryWork>();
        var userID = Guid.NewGuid().ToString();
        foreach (var item in model.SelectedCategoriesWork)
        {
            categories.Add(new MyUserCategoryWork()
            {
                CategoryWorkId = item,
                CreateRecordDate = now,
                LastEditRecordDate = now,
                LastEditUserId = userID,
                CreateUserId = userID,
                
            });
        }

        var myUserInfosToShow = new List<MyUserInfoToShow>();
        var infoToShow = _accountService.GetInfosToShow();
        foreach (var info in infoToShow)
        {
            myUserInfosToShow.Add(new MyUserInfoToShow()
            {
                Show = true,
                InfoToShowId = info.Id,
                CreateRecordDate = now,
                LastEditRecordDate = now,
                CreateUserId = userID,
                LastEditUserId = userID
            });
        }

        var newUser = new MyUser() 
        {
            CreateDate = now,
            Id = userID,
            Email = model.Email,
            UserName = model.Username,
            Name = model.Name,
            Surname = model.Surname,
            MyUserCategoriesWork = categories,
            ProfileImageFileName = imageDefault,
            LastLoginTime = DateTime.Now,
            MyUserSubscriptionPlans = new List<MyUserSubscriptionPlan>
            {
                new ()
                {
                    SubscriptionPlanId = _accountService.GetSubscriptionPlanByName(VeesyConstants.SubscriptionPlan.Free).Id,
                    CreateUserId = userID,
                    LastEditUserId = userID
                }
            },
            MyUserInfosToShow = myUserInfosToShow
        };
        
        var validate = await _myUserValidator.UserValidator(newUser);
        if (!validate.Success) 
            return validate;
        
        var adm = await _userManager.FindByEmailAsync(model.Email);
        if (adm != null) 
            return new ResultDto(false, "The e-mail entered has already been used.");
        
        adm = await _userManager.FindByNameAsync(model.Username);
        if (adm != null) 
            return new ResultDto(false, "The username entered has already been used.");
        
        IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

        if (!result.Succeeded)
        {
            return new ResultDto(false, result.Errors.FirstOrDefault().Description);
        }
        
        await _userManager.AddToRolesAsync(newUser, new[] { Roles.User });
        
        return new ResultDto(true, "User create correctly.");
    }

    public SignUpViewModel GetSignUpViewModel()
    {
        return new SignUpViewModel()
        {
            CategoriesWork = _accountService.GetCategoriesWork()
        };
    }

    public SignUpViewModel GetSignUpViewModelException(SignUpViewModel model)
    {
        model.CategoriesWork = _accountService.GetCategoriesWork();
        return model;
    }

    public void AddLastLogin(MyUser user)
    {
        try
        {
            user.LastLoginTime = DateTime.Now;
            _userManager.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, ex.Message);
        }
    }
    public static string SelectRandomImageName()
    {
        // Array di nomi di file
        string[] imageFiles = {
            "ProfilePicNew-01.jpg",
            "ProfilePicNew-02.jpg",
            "ProfilePicNew-03.jpg",
            "ProfilePicNew-04.jpg",
            "ProfilePicNew-05.jpg",
            "ProfilePicNew-06.jpg",
            "ProfilePicNew-07.jpg",
            "ProfilePicNew-08.jpg",
            "ProfilePicNew-09.jpg",
            "ProfilePicNew-10.jpg"
        };

        int randomIndex = random.Next(0, imageFiles.Length);
        
        string randomFileName = imageFiles[randomIndex];
        
        return randomFileName;
    }

}