using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Exception;
using Veesy.Domain.Models;
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
            ApplicationUrl = _config["ApplicationUrl"],
            Medias = mediasDto,
            Username = user.UserName,
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
        var vm = new EditViewModel()
        {
            Media = MapCloudDtos.MapMedia(mediaSelected),
            PreviousMedia =MapCloudDtos.MapMedia(previousMedia),
            NextMedia = MapCloudDtos.MapMedia(nextMedia),
            Username = userInfo.UserName,
            BasePath = $"{_config["ImagesKitIoEndpoint"]}{MediaCostants.BlobMediaSections.OriginalMedia}/"
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
        if(mediaDto.OriginalFileName.Contains("/"))
            return new ResultDto(false, "Characters '/' not allowed.");
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
}