using Microsoft.Extensions.Configuration;
using Veesy.Domain.Models;
using Veesy.Media.Constants;
using Veesy.Presentation.Model.Cloud;
using Veesy.Service.Dtos;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class CloudHelper
{
    private readonly IMediaService _mediaService;
    private readonly IConfiguration _config;

    public CloudHelper(IMediaService mediaService, IConfiguration config)
    {
        _mediaService = mediaService;
        _config = config;
    }

    public CloudViewModel GetCloudViewModel(MyUser user)
    {
        var medias = _mediaService.GetAllByUserId(user);
        var mediasDto = MapCloudDtos.MapMediaList(medias);
        return new CloudViewModel()
        {
            Medias = mediasDto,
            Username = user.UserName,
            BasePath = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/"
        };
    }
}