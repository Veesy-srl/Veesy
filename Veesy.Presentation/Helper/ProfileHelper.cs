using Veesy.Domain.Exception;
using Veesy.Domain.Models;
using Veesy.Presentation.Model.Account;
using Veesy.Service.Dtos;
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
            PortfolioIntro = userInfo.PortfolioIntro,
            UsedSoftwares = MapProfileDtos.MapUsedSoftwareList(_accountService.GetUsedSoftwareWithUser(userInfo))
        };
    }

    public async Task<ResultDto> UpdateUsedSoftware(List<Guid> usedSoftwareCodes, MyUser userInfo)
    {
        var oldUsedSoftware = _accountService.GetUsedSoftwaresByUser(userInfo);
        var usedSoftwareToDelete = new List<MyUserUsedSoftware>();
        var usedSoftwareToAdd = new List<MyUserUsedSoftware>();
        
        //Comparison of previous UsedSoftware with those currently selected to delete them
        foreach (var item in oldUsedSoftware)
        {
            if(!usedSoftwareCodes.Contains(item.UsedSoftwareId))
                usedSoftwareToDelete.Add(item);
        }

        foreach (var item in usedSoftwareCodes)
        {
            if(!oldUsedSoftware.Any(s => s.UsedSoftwareId == item))
                usedSoftwareToAdd.Add(new MyUserUsedSoftware()
                {
                    MyUserId = userInfo.Id,
                    UsedSoftwareId = item,
                    IsPrincipal = false
                });
        }

        return await _accountService.UpdateMyUserUsedSoftware(usedSoftwareToDelete, usedSoftwareToAdd);
    }
}