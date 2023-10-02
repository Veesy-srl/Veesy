using FluentValidation;
using NLog;
using NuGet.Protocol;
using Veesy.Domain.Models;
using Veesy.Domain.Exceptions;

namespace Veesy.Validators;

public class MyUserValidator : AbstractValidator<MyUser>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
    public MyUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter your name.");

        RuleFor(x => x.Surname).NotEmpty().WithMessage("Please enter your surname.");

        RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("Please enter your username.");
        
        RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter your valid email.");
    }
        
        
    public async Task<ResultDto> UserValidator(MyUser user)
    {
        try
        {
            var userValidation = await ValidateAsync(user);
            if(!userValidation.IsValid)
                return new ResultDto(false, userValidation.Errors.FirstOrDefault().ErrorMessage.Replace("'", "&#39;"));
            return new ResultDto(true, string.Empty);
        }
        catch (Exception e)
        {
            Logger.Error(e, $"Errore durante la validazione dello User: <{user.ToJson()}>");
            return new ResultDto(false, "Errore durante la validazione dell'utente.");
        }
    }
}