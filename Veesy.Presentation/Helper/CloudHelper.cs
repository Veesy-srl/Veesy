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
        
        var vm = new EditViewModel()
        {
            Portfolios = portfolios,
            Media = MapCloudDtos.MapMedia(mediaSelected),
            PreviousMedia =MapCloudDtos.MapMedia(previousMedia),
            NextMedia = MapCloudDtos.MapMedia(nextMedia),
            Username = userInfo.UserName,
            BasePath = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/",
            BasePathAzure = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/"
        };
        return (new ResultDto(true, ""), vm);
    }

    public async Task<ResultDto> UpdateFileName(MediaDto mediaDto, MyUser userInfo)
    {
        var media = _mediaService.GetMediaById(mediaDto.Code);
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
        var media = _mediaService.GetMediaById(mediaDto.Code);
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
        var vm = new SingleMediaViewModel()
        {
            Media = MapCloudDtos.MapMedia(mediaSelected),
            BasePath = $"{_config["ApplicationUrl"]}{_config["ImagesEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/"
        };
        return (new ResultDto(true, ""), vm);
    }
}