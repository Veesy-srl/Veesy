using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Presentation.Model.Account;
using Veesy.Service.Dtos;
using Veesy.Service.Interfaces;
using Veesy.Validators;

namespace Veesy.Presentation.Helper;

public class ProfileHelper
{
    private readonly IAccountService _accountService;
    private readonly MyUserValidator _myUserValidator;
    private readonly UserManager<MyUser> _userManager;
    private readonly MediaHelper _mediaHelper;
    private readonly IConfiguration _config;

    public ProfileHelper(IAccountService accountService, MyUserValidator myUserValidator, UserManager<MyUser> userManager, MediaHelper mediaHelper, IConfiguration config)
    {
        _accountService = accountService;
        _myUserValidator = myUserValidator;
        _userManager = userManager;
        _mediaHelper = mediaHelper;
        _config = config;
    }

    public async Task<ResultDto> UpdateMyUserBio(string biography, MyUser user)
    {
        if (biography.Length > 500)
            return new ResultDto(false, "Max characters is 500.");
        user.Biografy = string.IsNullOrEmpty(biography) ? null : biography;
        return await _accountService.UpdateUserProfile(user);
    }
    
    public async Task<ResultDto> UpdateMyUserPortfolioIntro(string introPortfolio, MyUser user)
    {
        if (introPortfolio.Length > 100)
            return new ResultDto(false, "Max characters is 100.");
        user.PortfolioIntro = string.IsNullOrEmpty(introPortfolio) ? null : introPortfolio;
        return await _accountService.UpdateUserProfile(user);
    }

    public ProfileViewModel GetProfileViewModel(MyUser userInfo)
    {
        var softskills = _accountService.GetSkillsWithUserByType(userInfo, SkillConstants.SoftSkill);
        return new ProfileViewModel()
        {
            FileName = userInfo.ProfileImageFileName,
            BasePathImages = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/",
            ExternalLink = userInfo.ExternalLink,
            Category = _accountService.GetCategoriesWorkByUser(userInfo),
            Role = userInfo.Category,
            PhoneNumber = userInfo.PhoneNumber,
            Username = userInfo.UserName,
            Email = userInfo.Email,
            FullName = userInfo.Name + " " + userInfo.Surname,
            Biography = userInfo.Biografy,
            PortfolioIntro = userInfo.PortfolioIntro,
            RolesWork = MapProfileDtos.MapRolesWorkList(_accountService.GetRolesWorkWithUser(userInfo.Id)),
            UsedSoftwares = MapProfileDtos.MapUsedSoftwareList(_accountService.GetUsedSoftwareWithUser(userInfo)),
            LanguagesSpoken = MapProfileDtos.MapLanguagesSpokenList(_accountService.GetLanguagesSpokenWithUser(userInfo)),
            InfoToShow = MapProfileDtos.MapInfoToShowList(_accountService.GetInfosToShowWithUser(userInfo)),
            SoftSkills = MapProfileDtos.MapSkillsList(softskills.ToList()),
            Sectors = MapProfileDtos.MapSectorList(_accountService.GetSectorsWithUser(userInfo.Id))
        };
    }

    public async Task<ResultDto> UpdateUsedSoftware(List<Guid> usedSoftwareCodes, MyUser userInfo)
    {
        if (usedSoftwareCodes != null && usedSoftwareCodes.Count > 10)
            return new ResultDto(false, "Select max 10 softwares.");
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

        return await _accountService.UpdateMyUserUsedSoftware(usedSoftwareToDelete, usedSoftwareToAdd, userInfo);
    }

    public async Task<ResultDto> UpdateSkill(List<Guid> skillsCodes, MyUser userInfo, char skillType)
    {
        if (skillsCodes != null)
        {
            if(skillType == SkillConstants.SoftSkill && skillsCodes.Count > 5)
                return new ResultDto(false, "Select max 5 soft skills.");
            if(skillType == SkillConstants.HardSkill && skillsCodes.Count > 5)
                return new ResultDto(false, "Select max 5 hard skills.");
        }
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
        
        return await _accountService.UpdateMyUserSkills(skillToDelete, skillToAdd, userInfo);

    }

    public async Task<ResultDto> UpdateMyUserExternalLink(string externalLink, MyUser userInfo)
    {
        userInfo.ExternalLink = string.IsNullOrEmpty(externalLink) ? null : externalLink;
        return await _accountService.UpdateUserProfile(userInfo);
    }

    public async Task<ResultDto> UpdateCategoriesWork(List<Guid> categoriesWorkCodes, MyUser userInfo)
    {
        if (categoriesWorkCodes != null && categoriesWorkCodes.Count > 3)
            return new ResultDto(false, "Select max 3 categories.");
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
        
        return await _accountService.UpdateMyUserCategoriesWork(categoryWorksToDelete, categoryWorksToAdd, userInfo);
    }
    
    
    public async Task<ResultDto> UpdateSectors(List<Guid> sectorsCodes, MyUser userInfo)
    {
        if (sectorsCodes != null && sectorsCodes.Count > 10)
            return new ResultDto(false, "Select max 10 fields.");
        var oldSectors = _accountService.GetSectorsByUser(userInfo).ToList();
        var sectorsToDelete = new List<MyUserSector>();
        var sectorsToAdd = new List<MyUserSector>();
        
        foreach (var item in oldSectors)
        {
            if(!sectorsCodes.Contains(item.SectorId))
                sectorsToDelete.Add(item);
        }

        foreach (var item in sectorsCodes)
        {
            if(!oldSectors.Any(s => s.SectorId == item))
                sectorsToAdd.Add(new MyUserSector()
                {
                    MyUserId = userInfo.Id,
                    SectorId = item,
                    Note = ""
                });
        }
        
        return await _accountService.UpdateMyUserSectors(sectorsToDelete, sectorsToAdd, userInfo);
    }

    public async Task<ResultDto> UpdateInfoToShow(List<Guid> infoToShowCodes, MyUser userInfo)
    {
        var oldInfoToShow = _accountService.GetInfosToShowByUser(userInfo).ToList();
        var infoToShowToDelete = new List<MyUserInfoToShow>();
        var infoToShowToAdd = new List<MyUserInfoToShow>();
        
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
        
        return await _accountService.UpdateMyUserInfoToShow(infoToShowToDelete, infoToShowToAdd, userInfo);
    }

    public async Task<ResultDto> UpdateLanguageSpoken(List<Guid> languagesSpokenCodes, MyUser userInfo)
    {
        if (languagesSpokenCodes != null && languagesSpokenCodes.Count > 5)
            return new ResultDto(false, "Select max 5 languages.");
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
        
        return await _accountService.UpdateMyUserLanguageSpoken(languagesToDelete, languagesToAdd, userInfo);
    }

    public async Task<ResultDto> UpdateFullName(string name, string surname, MyUser userInfo)
    {
        userInfo.Name = name;
        userInfo.Surname = surname;
        var result = await _myUserValidator.UserValidator(userInfo);
        if (!result.Success)
            return result;
        return await _accountService.UpdateUserProfile(userInfo);
    }
    public async Task<ResultDto> UpdateCategory(string category, MyUser userInfo)
    {
        if(category.Length > 100)
            return new ResultDto(false, "Max characters are 50.");
        userInfo.Category = category;
        return await _accountService.UpdateUserProfile(userInfo);
    }
    
    public async Task<ResultDto> UpdateVATNumber(string vatNumber, MyUser userInfo)
    {
        if(vatNumber.Length > 20)
            return new ResultDto(false, "Max characters are 20.");
        userInfo.VATNumber = vatNumber;
        return await _accountService.UpdateUserProfile(userInfo);
    }
    
    public async Task<ResultDto> UpdatePhoneNumber(string phoneNumberPrefix, string phoneNumber, MyUser userInfo)
    {
        if(phoneNumber.Length > 13)
            return new ResultDto(false, "Phone number max characters are 13.");
        if(string.IsNullOrEmpty(phoneNumberPrefix))
            return new ResultDto(false, "Please insert prefix.");

        userInfo.PhoneNumber = phoneNumber;
        userInfo.PhoneNumberPrefix = phoneNumberPrefix;
        return await _accountService.UpdateUserProfile(userInfo);
    }

    public BasicInfoViewModel GetBasicInfoViewModel(MyUser userInfo)
    {
        return new BasicInfoViewModel()
        {
            CategoriesWork = MapProfileDtos.MapCategoriesWorkList(_accountService.GetCategoriesWorkWithUser(userInfo.Id)),
            Email = userInfo.Email,
            Name = userInfo.Name,
            Surname = userInfo.Surname,
            Username = userInfo.UserName,
            Category = userInfo.Category,
            VatNumber = userInfo.VATNumber,
            PhoneNumber = userInfo.PhoneNumber,
            PhoneNumberPrefix = userInfo.PhoneNumberPrefix,
            FileName = userInfo.ProfileImageFileName,
            BasePathImages = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/",
        };
    }

    public async Task<ResultDto> UpdateEmail(string email, MyUser userInfo)
    {
        userInfo.Email = email;
        //TODO: Validate mail
        var result = await _userManager.UpdateAsync(userInfo);
        if (result.Succeeded)
            return new ResultDto(true, "");
        return new ResultDto(false, result.Errors.FirstOrDefault().Description);
    }
    
    public async Task<ResultDto> UpdateUsername(string username, MyUser userInfo)
    {
        userInfo.UserName = username;
        var result = await _userManager.UpdateAsync(userInfo);
        if (result.Succeeded)
            return new ResultDto(true, "");
        return new ResultDto(false, result.Errors.FirstOrDefault().Description);
    }
    
    public async Task<ResultDto> UpdatePassword(string oldPassword, string newPassword, MyUser userInfo)
    {
        var validCredentials = await _userManager.CheckPasswordAsync(userInfo, oldPassword);
        if (!validCredentials)
            return new ResultDto(false, "Old password is incorrect. Please retry");
        var token = await _userManager.GeneratePasswordResetTokenAsync(userInfo);
        var result = await _userManager.ResetPasswordAsync(userInfo, token, newPassword);
        if (result.Succeeded)
            return new ResultDto(true, "");
        return new ResultDto(false, result.Errors.FirstOrDefault().Description);
    }

    public async Task<ResultDto> UpdateProfileImage(Stream requestBody, string? requestContentType, MyUser userInfo)
    {
        var result = await _mediaHelper.UploadProfileImageOnAzure(requestBody, requestContentType);
        if (result.resultDto.Success)
        {
            userInfo.OrginalProfileImageName = result.originalFilename;
            userInfo.ProfileImageFileName = result.newFileName;
            return await _accountService.UpdateUserProfile(userInfo);
        }

        return result.resultDto;
    }

    public async Task<ResultDto> UpdateRolesWorks(List<Guid> roleCodes, MyUser userInfo)
    {
        if (roleCodes != null && roleCodes.Count > 5)
            return new ResultDto(false, "Select max 5 roles.");
        var oldRolesWork = _accountService.GetRolesWorkByUser(userInfo).ToList();
        var rolesToDelete = new List<MyUserRoleWork>();
        var rolesToAdd = new List<MyUserRoleWork>();
        
        foreach (var item in oldRolesWork)
        {
            if(!roleCodes.Contains(item.RoleWorkId))
                rolesToDelete.Add(item);
        }

        foreach (var item in roleCodes)
        {
            if(!oldRolesWork.Any(s => s.RoleWorkId == item))
                rolesToAdd.Add(new MyUserRoleWork()
                {
                    MyUserId = userInfo.Id,
                    RoleWorkId = item
                });
        }
        
        return await _accountService.UpdateMyUserRolesWork(rolesToDelete, rolesToAdd, userInfo);
    }
}