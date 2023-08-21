using Veesy.Domain.Exception;
using Veesy.Domain.Models;
using Veesy.Presentation.Model.Account;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class ProfileHelper
{
    private readonly IAccountService _accountService;

    public ProfileHelper(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<ResultDto> UpdateMyUserBio(string biography, MyUser user)
    {
        user.Biografy = string.IsNullOrEmpty(biography) ? null : biography;
        return await _accountService.UpdateUserProfile(user);
    }
    
    public async Task<ResultDto> UpdateMyUserPortfolioIntro(string introPortfolio, MyUser user)
    {
        user.PortfolioIntro = string.IsNullOrEmpty(introPortfolio) ? null : introPortfolio;
        return await _accountService.UpdateUserProfile(user);
    }

    public ProfileViewModel GetProfileViewModel(MyUser userInfo)
    {
        return new ProfileViewModel()
        {
            Biography = userInfo.Biografy,
            PortfolioIntro = userInfo.PortfolioIntro
        };
    }
}