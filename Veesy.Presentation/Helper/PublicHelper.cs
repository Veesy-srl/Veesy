using System.Collections.Immutable;
using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Presentation.Model.Cloud;
using Veesy.Service.Dtos;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class PublicHelper
{
    private readonly IMediaService _mediaService;
    private readonly IAccountService _accountService;
    private readonly IConfiguration _config;

    public PublicHelper(IMediaService mediaService, IAccountService accountService, IConfiguration config)
    {
        _mediaService = mediaService;
        _accountService = accountService;
        _config = config;
    }

    public AboutMediaViewModel GetUserMediaList(int count)
    {
        var UserList = _mediaService.GetRandomMediaWithUsername(count);
        
        AboutMediaViewModel List = new AboutMediaViewModel();
        List.ApplicationUrl = _config["ApplicationUrl"];
        List.BasePath = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/";
        List.BasePathImages = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/";
        List.MediaList = UserList.Select(item => item.Medias[0].FileName).ToList();
        List.MediaUser = UserList.Select(item => item.ProfileImageFileName).ToList();
        List.Usernames = UserList.Select(item => item.UserName).ToList();
        List.Id = UserList.Select(item => item.Id).ToList();
        List.PortfolioId = UserList.Select(item => item.Portfolios[0].Id).ToList();

        return List;
    }
    
    public CreatorsViewModel GetCreatorsViewModel()
    {
        List<MyUser> userInfo = _accountService.GetAllCreators().ToList();
        List<MyUserCategoryWork> categoryWorks = new List<MyUserCategoryWork>();

        foreach (var user in userInfo)
        {
            categoryWorks.AddRange(user.MyUserCategoriesWork);
        }
        
        return new CreatorsViewModel()
        {
            User = userInfo,
            CategoryWorks = categoryWorks.DistinctBy(s => s.CategoryWork.Name).Select(category => category.CategoryWork.Name).ToList(),
            BasePathImages = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/"
        };
    }
    
    public List<string> GetCreatorsFiltered(List<string> category)
    {

        var initialResults = _accountService.GetFilteredCreators(category);

        var usersWithAllCategories = initialResults
            .Where(u => category.All(category => u.MyUserCategoriesWork.Any(cw => cw.CategoryWork.Name == category)))
            .ToList();
        
        return usersWithAllCategories.Select(info => info.Id).ToList();
    }
}