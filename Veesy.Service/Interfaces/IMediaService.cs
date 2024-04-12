using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Service.Implementation;

namespace Veesy.Service.Interfaces;

public interface IMediaService
{
    List<Media> GetAllByUserId(MyUser user);
    Task<Media> AddMedia(Media media, MyUser user);
    Media? GetMediaById(Guid id);
    Media? GetPreviousMediaByDate(DateTime mediaSelectedCreateRecordDate, MyUser user);
    Media? GetNextMediaByDate(DateTime mediaSelectedCreateRecordDate, MyUser user);
    Task<ResultDto> UpdateMedia(Media media, MyUser user);
    long GetSizeMediaStorageByUserId(string userId);
    public List<Media> GetRandomMedia(int count);
    Task<ResultDto> DeleteMediasAndUpdatePortfolios(List<Media> medias, List<Portfolio> portfolios, MyUser user);
    Task<ResultDto> DeleteMediaAndUpdatePortfolios(Media media, List<Portfolio> portfolios, MyUser user);

    List<(string FileName, long Size)> GetMediasNameAndSizeByUserId(string userId);
    Media GetMediaByIdWithPortfoliosMedia(Guid imgCode);
    List<string> GetAllMediaNameByUser(MyUser userInfo);
    List<Media> GetMediasByIdWithPortfoliosMedia(List<Guid> imgToDelete);
    int GetMediaNumberByUser(MyUser user);
    int GetMediaNumber();
    List<MediaService.MediaOverviewDto> GetMediaNumberByMonthGroupByDay(int month, int year);
    List<Media> GetLastFourMediaUploaded();
    public List<Media> GetRandomPhotos(int count);

}