using Veesy.Domain.Exception;
using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface IAccountService
{
    Task<ResultDto> UpdateUserProfile(MyUser user);
    List<UsedSoftware> GetUsedSoftware();
    List<UsedSoftware> GetUsedSoftwareWithUser(MyUser user);
    List<MyUserUsedSoftware> GetUsedSoftwaresByUser(MyUser user);
    Task<ResultDto> UpdateMyUserUsedSoftware(List<MyUserUsedSoftware> usedSoftwareToDelete, List<MyUserUsedSoftware> usedSoftwareToAdd, MyUser user);
    IEnumerable<Skill> GetSkillsWithUserByType(MyUser userInfo, char type);
    IEnumerable<MyUserSkill> GetSkillsByUserAndType(MyUser user, char type);
    Task<ResultDto> UpdateMyUserSkills(List<MyUserSkill> skillToDelete, List<MyUserSkill> skillToAdd, MyUser user);
    List<CategoryWork> GetCategoriesWork();
    SubscriptionPlan GetSubscriptionPlanByName(string name);
    List<InfoToShow> GetInfosToShow();
    List<CategoryWork> GetCategoriesWorkWithUser(string userId);
    List<MyUserCategoryWork> GetCategoriesWorkByUser(MyUser userInfo);
    Task<ResultDto> UpdateMyUserCategoriesWork(List<MyUserCategoryWork> categoryWorksToDelete, List<MyUserCategoryWork> categoryWorksToAdd, MyUser user);
    List<MyUserInfoToShow> GetInfosToShowByUser(MyUser userInfo);
    Task<ResultDto> UpdateMyUserInfoToShow(List<MyUserInfoToShow> infoToShowToDelete, List<MyUserInfoToShow> infoToShowToAdd, MyUser user);
    List<MyUserLanguageSpoken> GetLanguageSpokenByUser(MyUser userInfo);
    Task<ResultDto> UpdateMyUserLanguageSpoken(List<MyUserLanguageSpoken> languageSpokenToDelete, List<MyUserLanguageSpoken> languageSpokenToAdd, MyUser user);
    List<InfoToShow> GetLanguagesSpokenWithUser(MyUser userInfo);
    List<LanguageSpoken> GetInfosToShowWithUser(MyUser userInfo);
}