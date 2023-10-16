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

    public List<(string ImgPath, string userImg, string Username)> GetListMedia(int count)
    {
        var mediaListWithUsernames = new List<(string, string, string)>();
       
        mediaListWithUsernames = _mediaService.GetRandomMediaWithUsername(count);

        return mediaListWithUsernames;
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