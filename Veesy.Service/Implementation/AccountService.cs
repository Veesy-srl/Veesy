using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Exception;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories;
using Veesy.Service.Interfaces;

namespace Veesy.Service.Implementation;

public class AccountService : IAccountService
{
    private readonly IVeesyUoW _uoW;

    public AccountService(IVeesyUoW uoW)
    {
        _uoW = uoW;
    }

    public async Task<ResultDto> UpdateUserProfile(MyUser user)
    {
        _uoW.MyUserRepository.Update(user);
        await _uoW.CommitAsync(user);
        return new ResultDto(true, "");
    }

    public List<UsedSoftware> GetUsedSoftware()
    {
        return _uoW.UsedSoftwareRepository.FindAll().ToList();
    }

    public List<UsedSoftware> GetUsedSoftwareWithUser(MyUser user)
    {
        return _uoW.UsedSoftwareRepository.FindAll().Include(s => s.MyUserUsedSoftwares.Where(s => s.MyUserId == user.Id)).OrderBy(s => s.Name).ToList();
    }

    public List<MyUserUsedSoftware> GetUsedSoftwaresByUser(MyUser user)
    {
        return _uoW.UsedSoftwareRepository.GetUsedSoftwaresByUser(user);
    }

    public async Task<ResultDto> UpdateMyUserUsedSoftware(List<MyUserUsedSoftware> usedSoftwareToDelete, List<MyUserUsedSoftware> usedSoftwareToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.UsedSoftwareRepository.DeleteMyUserUsedSoftwares(usedSoftwareToDelete);
                await _uoW.UsedSoftwareRepository.AddMyUserUsedSoftwares(usedSoftwareToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public IEnumerable<Skill> GetSkillsWithUserByType(MyUser userInfo, char type)
    {
        return _uoW.SkillRepository.FindAll().Include(s => s.MyUserSkills.Where(s => s.MyUserId == userInfo.Id && s.Type == type)).OrderBy(s => s.Name);
    }

    public IEnumerable<MyUserSkill> GetSkillsByUserAndType(MyUser user, char type)
    {
        return _uoW.SkillRepository.GetSkillsByUserAndType(user, type);
    }

    public async Task<ResultDto> UpdateMyUserSkills(List<MyUserSkill> skillToDelete, List<MyUserSkill> skillToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.SkillRepository.DeleteMyUserSkills(skillToDelete);
                await _uoW.SkillRepository.AddMyUserSkills(skillToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public List<CategoryWork> GetCategoriesWork()
    {
        return _uoW.MyUserRepository.GetCategoriesWork();
    }

    public SubscriptionPlan GetSubscriptionPlanByName(string name)
    {
        return _uoW.MyUserRepository.GetSubscriptionPlanByName(name);
    }

    public List<InfoToShow> GetInfosToShow()
    {
        return _uoW.MyUserRepository.GetInfosToShow();
    }

    public List<CategoryWork> GetCategoriesWorkWithUser(string userId)
    {
        return _uoW.MyUserRepository.GetCategoriesWorkByUserId(userId);
    }

    public List<MyUserCategoryWork> GetCategoriesWorkByUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetCategoriesWorkByUser(userInfo);
    }

    public async Task<ResultDto> UpdateMyUserCategoriesWork(List<MyUserCategoryWork> categoryWorksToDelete, List<MyUserCategoryWork> categoryWorksToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.MyUserRepository.DeleteMyUserCategoriesWork(categoryWorksToDelete);
                await _uoW.MyUserRepository.AddMyUserCategoriesWork(categoryWorksToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public List<MyUserInfoToShow> GetInfosToShowByUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetInfosToShowByUser(userInfo);
    }

    public async Task<ResultDto> UpdateMyUserInfoToShow(List<MyUserInfoToShow> infoToShowToDelete, List<MyUserInfoToShow> infoToShowToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.MyUserRepository.DeleteMyUserInfoToShow(infoToShowToDelete);
                await _uoW.MyUserRepository.AddMyUserInfoToShow(infoToShowToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public List<MyUserLanguageSpoken> GetLanguageSpokenByUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetLanguageSpokenByUser(userInfo);
    }

    public async Task<ResultDto> UpdateMyUserLanguageSpoken(List<MyUserLanguageSpoken> languageSpokenToDelete, List<MyUserLanguageSpoken> languageSpokenToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.MyUserRepository.DeleteMyUserLanguageSpoken(languageSpokenToDelete);
                await _uoW.MyUserRepository.AddMyUserLanguageSpoken(languageSpokenToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public List<LanguageSpoken> GetLanguagesSpokenWithUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetLanguagesSpokenByUserId(userInfo.Id);
    }

    public List<InfoToShow> GetInfosToShowWithUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetInfoToShowByUserId(userInfo.Id);
    }
}