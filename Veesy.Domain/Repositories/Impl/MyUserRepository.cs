using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories.Impl;

public class MyUserRepository : RepositoryBase<MyUser>, IMyUserRepository 
{
    public MyUserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public List<CategoryWork> GetCategoriesWork()
    {
        return _applicationDbContext.CategoriesWork.ToList();
    }

    public SubscriptionPlan GetSubscriptionPlanByName(string name)
    {
        return _applicationDbContext.SubscriptionPlans.SingleOrDefault(s => s.Name == name);
    }

    public List<InfoToShow> GetInfosToShow()
    {
        return _applicationDbContext.InfosToShow.ToList();
    }

    public List<CategoryWork> GetCategoriesWorkByUserId(string userId)
    {
        return _applicationDbContext.CategoriesWork.Include(s => s.MyUserCategoriesWork.Where(s => s.MyUserId == userId)).ToList();

    }

    public List<MyUserCategoryWork> GetCategoriesWorkByUser(MyUser userInfo)
    {
        return _applicationDbContext.MyUserCategoriesWork
            .Include(s => s.CategoryWork)
            .Where(s => s.MyUserId == userInfo.Id)
            .ToList();
    }

    public void DeleteMyUserCategoriesWork(List<MyUserCategoryWork> categoryWorksToDelete)
    {
        _applicationDbContext.MyUserCategoriesWork.RemoveRange(categoryWorksToDelete);
    }

    public async Task AddMyUserCategoriesWork(List<MyUserCategoryWork> categoryWorksToAdd)
    {
        await _applicationDbContext.MyUserCategoriesWork.AddRangeAsync(categoryWorksToAdd);
    }

    public List<MyUserInfoToShow> GetInfosToShowByUser(MyUser userInfo)
    {
        return _applicationDbContext.MyUserInfosToShow
            .Include(s => s.InfoToShow)
            .Where(s => s.MyUserId == userInfo.Id)
            .ToList();
    }

    public void DeleteMyUserInfoToShow(List<MyUserInfoToShow> infoToShowToDelete)
    {
        _applicationDbContext.MyUserInfosToShow.RemoveRange(infoToShowToDelete);
    }

    public async Task AddMyUserInfoToShow(List<MyUserInfoToShow> infoToShowToAdd)
    {
        await _applicationDbContext.MyUserInfosToShow.AddRangeAsync(infoToShowToAdd);
    }

    public List<MyUserLanguageSpoken> GetLanguageSpokenByUser(MyUser userInfo)
    {
        return _applicationDbContext.MyUserLanguagesSpoken
            .Include(s => s.LanguageSpoken)
            .Where(s => s.MyUserId == userInfo.Id)
            .ToList();
    }

    public void DeleteMyUserLanguageSpoken(List<MyUserLanguageSpoken> languageSpokenToDelete)
    {
        _applicationDbContext.MyUserLanguagesSpoken.RemoveRange(languageSpokenToDelete);
    }

    public async Task AddMyUserLanguageSpoken(List<MyUserLanguageSpoken> languageSpokenToAdd)
    {
        await _applicationDbContext.MyUserLanguagesSpoken.AddRangeAsync(languageSpokenToAdd);
    }

    public List<InfoToShow> GetLanguagesSpokenByUserId(string userInfoId)
    {
        return _applicationDbContext.InfosToShow.Include(s => s.MyUserInfoToShows.Where(s => s.MyUserId == userInfoId)).ToList();
    }

    public List<LanguageSpoken> GetInfoToShowByUserId(string userInfoId)
    {
        return _applicationDbContext.LanguagesSpoken.Include(s => s.MyUserLanguagesSpoken.Where(s => s.MyUserId == userInfoId)).ToList();

    }
}