using Veesy.Domain.Exceptions;
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
    List<Sector> GetSectors();
    SubscriptionPlan GetSubscriptionPlanByName(string name);
    List<InfoToShow> GetInfosToShow();
    List<CategoryWork> GetCategoriesWorkWithUser(string userId);
    List<RoleWork> GetRolesWorkWithUser(string userId);
    List<Sector> GetSectorsWithUser(string userId);
    List<MyUserCategoryWork> GetCategoriesWorkByUser(MyUser userInfo);
    List<MyUserRoleWork> GetRolesWorkByUser(MyUser userInfo);
    List<MyUserSector> GetSectorsByUser(MyUser userInfo);
    Task<ResultDto> UpdateMyUserCategoriesWork(List<MyUserCategoryWork> categoryWorksToDelete, List<MyUserCategoryWork> categoryWorksToAdd, MyUser user);
    Task<ResultDto> UpdateMyUserRolesWork(List<MyUserRoleWork> rolesWorksToDelete, List<MyUserRoleWork> rolesWorksToAdd, MyUser user);
    Task<ResultDto> UpdateMyUserSectors(List<MyUserSector> sectorsToDelete, List<MyUserSector> sectorsToAdd, MyUser user);
    List<MyUserInfoToShow> GetInfosToShowByUser(MyUser userInfo);
    Task<ResultDto> UpdateMyUserInfoToShow(List<MyUserInfoToShow> infoToShowToDelete, List<MyUserInfoToShow> infoToShowToAdd, MyUser user);
    List<MyUserLanguageSpoken> GetLanguageSpokenByUser(MyUser userInfo);
    Task<ResultDto> UpdateMyUserLanguageSpoken(List<MyUserLanguageSpoken> languageSpokenToDelete, List<MyUserLanguageSpoken> languageSpokenToAdd, MyUser user);
    List<LanguageSpoken> GetLanguagesSpokenWithUser(MyUser userInfo);
    List<InfoToShow> GetInfosToShowWithUser(MyUser userInfo);
    List<MyUser> GetAllCreators();
}