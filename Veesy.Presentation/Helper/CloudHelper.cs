using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Presentation.Model.Cloud;
using Veesy.Service.Dtos;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class CloudHelper
{
    private readonly IMediaService _mediaService;
    private readonly IConfiguration _config;
    private readonly IPortfolioService _portfolioService;

    public CloudHelper(IMediaService mediaService, IConfiguration config, IPortfolioService portfolioService)
    {
        _mediaService = mediaService;
        _config = config;
        _portfolioService = portfolioService;
    }

    public CloudViewModel GetCloudViewModel(MyUser user)
    {
        var medias = _mediaService.GetAllByUserId(user);
        var mediasDto = MapCloudDtos.MapMediaList(medias);
        foreach (var media in mediasDto)
        {
            if (media.NestedPortfolioLinks != null && media.NestedPortfolioLinks != Guid.Empty)
            {
                media.NestedPortfolioNameForUrl = _portfolioService
                    .GetPortfolioById(media.NestedPortfolioLinks.Value, media.UserId).Name.ToLower().Replace(" ", "-").Replace("/", "-");
            }
        }
        return new CloudViewModel()
        {
            ApplicationUrl = _config["ApplicationUrl"],
            Medias = mediasDto,
            Username = user.UserName,
            Portfolios = MapPortfolioDtos.MapListPortfolioThumbnailDto(_portfolioService.GetPortfoliosByUserWithMedia(user).ToList()),
            BasePath = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/"
        };
    }

    public (ResultDto resultDto, EditViewModel? viewModel) GetEditViewModel(Guid id, MyUser userInfo)
    {
        var mediaSelected = _mediaService.GetMediaById(id);
        if (mediaSelected == null)
            return (new ResultDto(false, "Media selected not found."), null);

        var previousMedia = _mediaService.GetPreviousMediaByDate(mediaSelected.CreateRecordDate, userInfo);
        var nextMedia = _mediaService.GetNextMediaByDate(mediaSelected.CreateRecordDate, userInfo);

        var portfolios = _portfolioService.GetPortfoliosByUser(userInfo).ToList();

        var mediaDto = MapCloudDtos.MapMedia(mediaSelected);
        if (mediaDto.NestedPortfolioLinks != null && mediaDto.NestedPortfolioLinks != Guid.Empty)
        {
            mediaDto.NestedPortfolioNameForUrl = _portfolioService
                .GetPortfolioById(mediaDto.NestedPortfolioLinks.Value, mediaDto.UserId).Name.ToLower().Replace(" ", "-").Replace("/", "-");
        }
        
        var previousMediaDto = MapCloudDtos.MapMedia(previousMedia);
        if (previousMediaDto != null && previousMediaDto.NestedPortfolioLinks != null && previousMediaDto.NestedPortfolioLinks != Guid.Empty)
        {
            previousMediaDto.NestedPortfolioNameForUrl = _portfolioService
                .GetPortfolioById(previousMediaDto.NestedPortfolioLinks.Value, previousMediaDto.UserId).Name.ToLower().Replace(" ", "-").Replace("/", "-");
        }
        
        var nextMediaDto = MapCloudDtos.MapMedia(nextMedia);
        if (nextMediaDto != null && nextMediaDto.NestedPortfolioLinks != null && nextMediaDto.NestedPortfolioLinks != Guid.Empty)
        {
            nextMediaDto.NestedPortfolioNameForUrl = _portfolioService
                .GetPortfolioById(nextMediaDto.NestedPortfolioLinks.Value, nextMediaDto.UserId).Name.ToLower().Replace(" ", "-").Replace("/", "-");
        }
        
        var vm = new EditViewModel()
        {
            Portfolios = portfolios,
            Media = mediaDto,
            PreviousMedia = previousMediaDto,
            NextMedia = nextMediaDto,
            Username = userInfo.UserName,
            BasePath = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/",
            BasePathAzure = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/",
            BasePathCloud = $"{_config["ApplicationUrl"]}/cloud/"
        };
        return (new ResultDto(true, ""), vm);
    }

    public async Task<ResultDto> UpdateFileName(MediaDto mediaDto, MyUser userInfo)
    {
        var media = _mediaService.GetMediaByIdForUpdate(mediaDto.Code);
        if (media.MyUserId != userInfo.Id)
            return new ResultDto(false, "Transaction not allowed.");
        if(string.IsNullOrEmpty(mediaDto.OriginalFileName))
            return new ResultDto(false, "Please insert filename.");
        if(!mediaDto.OriginalFileName.Replace(" ", "").All(char.IsLetterOrDigit))
            return new ResultDto(false, "Only alphanumeric characters are allowed.");
        var names = _mediaService.GetAllMediaNameByUser(userInfo);
        if(names.Contains(mediaDto.OriginalFileName))
            return new ResultDto(false, $"Filename {mediaDto.OriginalFileName} already used.");
        media.OriginalFileName = mediaDto.OriginalFileName + media.Type;
        return await _mediaService.UpdateMedia(media, userInfo);
    }

    public async Task<ResultDto> UpdateFileUpdateCredits(MediaDto mediaDto, MyUser userInfo)
    {
        var media = _mediaService.GetMediaByIdForUpdate(mediaDto.Code);
        if (media.MyUserId != userInfo.Id)
            return new ResultDto(false, "Transaction not allowed.");
        media.Credits = mediaDto.Credits;
        return await _mediaService.UpdateMedia(media, userInfo);
    }

    public (ResultDto resultDto, SingleMediaViewModel? viewModel) GetSingleMediaViewModel(Guid id, MyUser userInfo)
    {
        var mediaSelected = _mediaService.GetMediaById(id);
        if (mediaSelected == null)
            return (new ResultDto(false, "Media selected not found."), null);
        
        var mediaDto = MapCloudDtos.MapMedia(mediaSelected);
        if (mediaDto.NestedPortfolioLinks != null && mediaDto.NestedPortfolioLinks != Guid.Empty)
        {
            mediaDto.NestedPortfolioNameForUrl = _portfolioService
                .GetPortfolioById(mediaDto.NestedPortfolioLinks.Value, mediaDto.UserId).Name.ToLower().Replace("/", "-").Replace(" ", "-");
        }
        
        var vm = new SingleMediaViewModel()
        {
            Media = mediaDto,
            BasePath = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/"
        };
        return (new ResultDto(true, ""), vm);
    }
}