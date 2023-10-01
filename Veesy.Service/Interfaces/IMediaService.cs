using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface IMediaService
{
    List<Media> GetAllByUserId(MyUser user);
    Task AddMedia(Media media, MyUser user);
    Media? GetMediaById(Guid id);
    Media? GetPreviousMediaByDate(DateTime mediaSelectedCreateRecordDate, MyUser user);
    Media? GetNextMediaByDate(DateTime mediaSelectedCreateRecordDate, MyUser user);
    Task<ResultDto> UpdateMedia(Media media, MyUser user);
}