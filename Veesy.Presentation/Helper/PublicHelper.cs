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
        List.BasePath = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/";
        List.BasePathImages = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/";
        List.MediaList = UserList.Select(item => item.Medias[0].FileName).ToList();
        List.MediaUser = UserList.Select(item => item.ProfileImageFileName).ToList();
        List.Usernames = UserList.Select(item => item.UserName).ToList();
        List.Id = UserList.Select((item => item.Id)).ToList();

        return List;
    }
    
    public CreatorsViewModel GetCreatorsViewModel()
    {
        var userInfo = _accountService.GetAllCreators().ToList();
        return new CreatorsViewModel()
        {
            Id = userInfo.Select(item => item.Id).ToList(),
            FileNameImage =  userInfo.Select(item => item.ProfileImageFileName).ToList(),
            BasePathImages = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.ProfileMedia}/",
            Role = userInfo.Select(item => item.Category).ToList(),
            Username = userInfo.Select(item => item.UserName).ToList(),
        };
    }
}