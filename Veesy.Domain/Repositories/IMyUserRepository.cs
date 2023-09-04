using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories;

public interface IMyUserRepository : IRepositoryBase<MyUser>
{
    List<CategoryWork> GetCategoriesWork();
    SubscriptionPlan GetSubscriptionPlanByName(string name);
    List<InfoToShow> GetInfosToShow();
    List<CategoryWork> GetCategoriesWorkByUserId(string userId);
    List<MyUserCategoryWork> GetCategoriesWorkByUser(MyUser userInfo);
    void DeleteMyUserCategoriesWork(List<MyUserCategoryWork> categoryWorksToDelete);
    Task AddMyUserCategoriesWork(List<MyUserCategoryWork> categoryWorksToAdd);
    List<MyUserInfoToShow> GetInfosToShowByUser(MyUser userInfo);
    void DeleteMyUserInfoToShow(List<MyUserInfoToShow> infoToShowToDelete);
    Task AddMyUserInfoToShow(List<MyUserInfoToShow> infoToShowToAdd);    
    List<MyUserLanguageSpoken> GetLanguageSpokenByUser(MyUser userInfo);
    void DeleteMyUserLanguageSpoken(List<MyUserLanguageSpoken> languageSpokenToDelete);
    Task AddMyUserLanguageSpoken(List<MyUserLanguageSpoken> languageSpokenToAdd);
    List<InfoToShow> GetLanguagesSpokenByUserId(string userInfoId);
    List<LanguageSpoken> GetInfoToShowByUserId(string userInfoId);
}