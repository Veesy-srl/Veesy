using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;
using System.Linq;
using Veesy.Domain.Constants;

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
    
    public List<Sector> GetSectors()
    {
        return _applicationDbContext.Sectors.ToList();
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
        return _applicationDbContext.CategoriesWork.Include(s => s.MyUserCategoriesWork.Where(s => s.MyUserId == userId)).OrderBy(s => s.Name).ToList();
    }
    
    public List<RoleWork> GetRolesWorkByUserId(string userId)
    {
        return _applicationDbContext.RolesWork.Include(s => s.MyUserRolesWork.Where(s => s.MyUserId == userId)).OrderBy(s => s.Name).ToList();
    }
    
    public List<Sector> GetSectorsByUserId(string userId)
    {
        return _applicationDbContext.Sectors.Include(s => s.MyUserSectors.Where(s => s.MyUserId == userId)).OrderBy(s => s.Name).ToList();

    }

    public List<MyUserCategoryWork> GetCategoriesWorkByUser(MyUser userInfo)
    {
        return _applicationDbContext.MyUserCategoriesWork
            .Include(s => s.CategoryWork)
            .Where(s => s.MyUserId == userInfo.Id)
            .OrderBy(s => s.CategoryWork.Name)
            .ToList();
    }

    public List<MyUserRoleWork> GetRolesWorkByUser(MyUser userInfo)
    {
        return _applicationDbContext.MyUserRolesWork
            .Include(s => s.RoleWork)
            .Where(s => s.MyUserId == userInfo.Id)
            .OrderBy(s => s.RoleWork.Name)
            .ToList();
    }
    
    public List<MyUserSector> GetSectorsByUser(MyUser userInfo)
    {
        return _applicationDbContext.MyUserSectors
            .Include(s => s.Sector)
            .Where(s => s.MyUserId == userInfo.Id)
            .OrderBy(s => s.Sector.Name)
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

    public void DeleteMyUserRolesWork(List<MyUserRoleWork> rolesWorksToDelete)
    {
        _applicationDbContext.MyUserRolesWork.RemoveRange(rolesWorksToDelete);
    }

    public async Task AddMyUserRolesWork(List<MyUserRoleWork> rolesWorksToAdd)
    {
        await _applicationDbContext.MyUserRolesWork.AddRangeAsync(rolesWorksToAdd);
    }

    public void DeleteMyUserSectors(List<MyUserSector> sectorsToDelete)
    {
        _applicationDbContext.MyUserSectors.RemoveRange(sectorsToDelete);
    }

    public async Task AddMyUserSectors(List<MyUserSector> sectorsToAdd)
    {
        await _applicationDbContext.MyUserSectors.AddRangeAsync(sectorsToAdd);
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
            .OrderBy(s => s.LanguageSpoken.Language)
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

    public List<LanguageSpoken> GetLanguagesSpokenByUserId(string userInfoId)
    {
        return _applicationDbContext.LanguagesSpoken.Include(s => s.MyUserLanguagesSpoken.Where(s => s.MyUserId == userInfoId)).OrderBy(s => s.Language).ToList();
    }

    public List<InfoToShow> GetInfoToShowByUserId(string userInfoId)
    {
        return _applicationDbContext.InfosToShow.Include(s => s.MyUserInfoToShows.Where(s => s.MyUserId == userInfoId)).OrderBy(s => s.Info).ToList();
    }

    public List<MyUser> GetAllUsersWithMainPortfolio()
    {
        var users = _applicationDbContext.MyUsers
            .Where(u => u.Portfolios.Any(p => p.IsMain && p.IsPublic && p.Status == 1))
            .Include(u => u.Portfolios)
            .Include(t => t.MyUserCategoriesWork)
            .ThenInclude(g => g.CategoryWork)
            .ToList();
        
        foreach (var user in users)
        {
            user.Portfolios = user.Portfolios
                .OrderByDescending(p => p.IsMain && p.IsPublic && p.Status == 1)
                .ToList();
        }

        return users;
    }

    public List<Media> GetRandomMedia(int count)
    {
        var portfolios =  _applicationDbContext.Portfolios
            .Include(s => s.PortfolioMedias)
            .ThenInclude(s => s.Media)
            .Include(s => s.MyUser)
            .Where(s => s.IsMain && s.IsPublic && s.Status == 1)
            .OrderBy(s => new Guid())
            .ToList();
        
        var countPerAuthor = count / portfolios.Count;
        if (portfolios.Count > 50)
        {
            countPerAuthor = 1;
        }
        var random = new Random();
        var result = new List<PortfolioMedia>();

        foreach (var group in portfolios)
        {
            var randomItems = group.PortfolioMedias.OrderBy(item => random.Next()).Take(countPerAuthor);
            result.AddRange(randomItems);
        }

        var resultMedia = result.Select(s => s.Media).OrderBy(s => random.Next()).Take(count).ToList();
        resultMedia.ForEach(s => s.MyUser.Portfolios = null);
        resultMedia.ForEach(s => s.MyUser.Medias = null);
        
        return resultMedia;
    }
    
    public List<Media> GetRandomPhotos(int count)
    {
        var portfolios = _applicationDbContext.Portfolios
            .Include(s => s.PortfolioMedias)
            .ThenInclude(s => s.Media)
            .Include(s => s.MyUser)
            .Where(s => s.IsMain && s.IsPublic && s.Status == 1)
            .OrderBy(s => new Guid())
            .ToList();

        var random = new Random();
        var countPerAuthor = count / portfolios.Count; 
        if (portfolios.Count > count)
        {
            countPerAuthor = 1;
        }
        var result = new List<PortfolioMedia>();

        foreach (var group in portfolios)
        {
            var randomPhotos = group.PortfolioMedias.OrderBy(item => random.Next()).Take(countPerAuthor);
            result.AddRange(randomPhotos);
        }

        var resultMedia = result.Select(s => s.Media).OrderBy(s => random.Next()).Where(s => MediaCostants.ImageExtensions.Contains(s.Type.ToUpper())).Take(count).ToList();
        
        resultMedia.ForEach(s => s.MyUser.Portfolios = null);
        resultMedia.ForEach(s => s.MyUser.Medias = null);

        return resultMedia;
    }
    
    public static List<T> Shuffle<T>(List<T> list)
    {
        var random = new Random();
        int n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        return list;
    }

}