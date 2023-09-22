using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories;

public interface IMyUserRepository : IRepositoryBase<MyUser>
{
    List<CategoryWork> GetCategoriesWork();
    SubscriptionPlan GetSubscriptionPlanByName(string name);
    List<InfoToShow> GetInfosToShow();
    List<CategoryWork> GetCategoriesWorkByUserId(string userId);
    List<Sector> GetSectorsByUserId(string userId);
    List<MyUserCategoryWork> GetCategoriesWorkByUser(MyUser userInfo);
    List<MyUserSector> GetSectorsByUser(MyUser userInfo);
    void DeleteMyUserCategoriesWork(List<MyUserCategoryWork> categoryWorksToDelete);
    Task AddMyUserCategoriesWork(List<MyUserCategoryWork> categoryWorksToAdd);
    void DeleteMyUserSectors(List<MyUserSector> sectorsToDelete);
    Task AddMyUserSectors(List<MyUserSector> sectorsToAdd);
    List<MyUserInfoToShow> GetInfosToShowByUser(MyUser userInfo);
    void DeleteMyUserInfoToShow(List<MyUserInfoToShow> infoToShowToDelete);
    Task AddMyUserInfoToShow(List<MyUserInfoToShow> infoToShowToAdd);    
    List<MyUserLanguageSpoken> GetLanguageSpokenByUser(MyUser userInfo);
    void DeleteMyUserLanguageSpoken(List<MyUserLanguageSpoken> languageSpokenToDelete);
    Task AddMyUserLanguageSpoken(List<MyUserLanguageSpoken> languageSpokenToAdd);
    List<LanguageSpoken> GetLanguagesSpokenByUserId(string userInfoId);
    List<InfoToShow> GetInfoToShowByUserId(string userInfoId);
    List<Sector> GetSectors();
}