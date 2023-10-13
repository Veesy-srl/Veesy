using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;

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
    Task DeleteMedia(Media media, MyUser user);
    public List<(string, string, string)> GetRandomMediaWithUsername(int count);

    List<(string FileName, long Size)> GetMediasNameAndSizeByUserId(string userId);
}