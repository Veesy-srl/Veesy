using Veesy.Domain.Constants;
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
        var hardskills = _accountService.GetSkillsWithUserByType(userInfo, SkillConstants.HardSkill);
        var softskills = _accountService.GetSkillsWithUserByType(userInfo, SkillConstants.SoftSkill);
        return new ProfileViewModel()
        {
            ExternalLink = userInfo.ExternalLink,
            PhoneNumber = userInfo.PhoneNumber,
            Username = userInfo.UserName,
            Email = userInfo.Email,
            FullName = userInfo.Name + " " + userInfo.Surname,
            Biography = userInfo.Biografy,
            PortfolioIntro = userInfo.PortfolioIntro,
            CategoriesWork = MapProfileDtos.MapCategoriesWorkList(_accountService.GetCategoriesWorkWithUser(userInfo.Id)),
            UsedSoftwares = MapProfileDtos.MapUsedSoftwareList(_accountService.GetUsedSoftwareWithUser(userInfo)),
            LanguagesSpoken = MapProfileDtos.MapLanguagesSpokenList(_accountService.GetInfosToShowWithUser(userInfo)),
            InfoToShow = MapProfileDtos.MapInfoToShowList(_accountService.GetLanguagesSpokenWithUser(userInfo)),
            HardSkills = MapProfileDtos.MapSkillsList(hardskills.ToList()),
            SoftSkills = MapProfileDtos.MapSkillsList(softskills.ToList())
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

    public async Task<ResultDto> UpdateSkill(List<Guid> skillsCodes, MyUser userInfo, char skillType)
    {
        var oldSkills = _accountService.GetSkillsByUserAndType(userInfo, skillType).ToList();
        var skillToDelete = new List<MyUserSkill>();
        var skillToAdd = new List<MyUserSkill>();
        
        //Comparison of previous UsedSoftware with those currently selected to delete them
        foreach (var item in oldSkills)
        {
            if(!skillsCodes.Contains(item.SkillId))
                skillToDelete.Add(item);
        }

        foreach (var item in skillsCodes)
        {
            if(!oldSkills.Any(s => s.SkillId == item))
                skillToAdd.Add(new MyUserSkill()
                {
                    Id = Guid.NewGuid(),
                    MyUserId = userInfo.Id,
                    SkillId = item,
                    Type = skillType,
                    IsPrincipal = false
                });
        }
        
        return await _accountService.UpdateMyUserSkills(skillToDelete, skillToAdd);

    }

    public async Task<ResultDto> UpdateMyUserExternalLink(string externalLink, MyUser userInfo)
    {
        userInfo.ExternalLink = string.IsNullOrEmpty(externalLink) ? null : externalLink;
        return await _accountService.UpdateUserProfile(userInfo);
    }

    public async Task<ResultDto> UpdateCategoriesWork(List<Guid> categoriesWorkCodes, MyUser userInfo)
    {
        var oldCategoriesWork = _accountService.GetCategoriesWorkByUser(userInfo).ToList();
        var categoryWorksToDelete = new List<MyUserCategoryWork>();
        var categoryWorksToAdd = new List<MyUserCategoryWork>();
        
        //Comparison of previous CategoriesWork with those currently selected to delete them
        foreach (var item in oldCategoriesWork)
        {
            if(!categoriesWorkCodes.Contains(item.CategoryWorkId))
                categoryWorksToDelete.Add(item);
        }

        foreach (var item in categoriesWorkCodes)
        {
            if(!oldCategoriesWork.Any(s => s.CategoryWorkId == item))
                categoryWorksToAdd.Add(new MyUserCategoryWork()
                {
                    MyUserId = userInfo.Id,
                    CategoryWorkId = item
                });
        }
        
        return await _accountService.UpdateMyUserCategoriesWork(categoryWorksToDelete, categoryWorksToAdd);
    }

    public async Task<ResultDto> UpdateInfoToShow(List<Guid> infoToShowCodes, MyUser userInfo)
    {
        var oldInfoToShow = _accountService.GetInfosToShowByUser(userInfo).ToList();
        var infoToShowToDelete = new List<MyUserInfoToShow>();
        var infoToShowToAdd = new List<MyUserInfoToShow>();
        
        //Comparison of previous CategoriesWork with those currently selected to delete them
        foreach (var item in oldInfoToShow)
        {
            if(!infoToShowCodes.Contains(item.InfoToShowId))
                infoToShowToDelete.Add(item);
        }

        foreach (var item in infoToShowCodes)
        {
            if(!oldInfoToShow.Any(s => s.InfoToShowId == item))
                infoToShowToAdd.Add(new MyUserInfoToShow()
                {
                    MyUserId = userInfo.Id,
                    InfoToShowId = item
                });
        }
        
        return await _accountService.UpdateMyUserInfoToShow(infoToShowToDelete, infoToShowToAdd);
    }

    public async Task<ResultDto> UpdateLanguageSpoken(List<Guid> languagesSpokenCodes, MyUser userInfo)
    {
        var oldLanguageSpoken = _accountService.GetLanguageSpokenByUser(userInfo).ToList();
        var languagesToDelete = new List<MyUserLanguageSpoken>();
        var languagesToAdd = new List<MyUserLanguageSpoken>();
        
        //Comparison of previous CategoriesWork with those currently selected to delete them
        foreach (var item in oldLanguageSpoken)
        {
            if(!languagesSpokenCodes.Contains(item.LanguageSpokenId))
                languagesToDelete.Add(item);
        }

        foreach (var item in languagesSpokenCodes)
        {
            if(!oldLanguageSpoken.Any(s => s.LanguageSpokenId == item))
                languagesToAdd.Add(new MyUserLanguageSpoken()
                {
                    MyUserId = userInfo.Id,
                    LanguageSpokenId = item
                });
        }
        
        return await _accountService.UpdateMyUserLanguageSpoken(languagesToDelete, languagesToAdd);
    }

    public async Task<ResultDto> UpdateFullName(string name, string surname, MyUser userInfo)
    {
        throw new NotImplementedException();
    }

    public BasicInfoViewModel GetBasicInfoViewModel(MyUser userInfo)
    {
        return new BasicInfoViewModel()
        {
            Email = userInfo.Email,
            Name = userInfo.Name,
            Surname = userInfo.Surname,
            Username = userInfo.UserName,
            Category = userInfo.Category,
            VatNumber = userInfo.VATNumber,
            PhoneNumber = userInfo.PhoneNumber
        };
    }
}