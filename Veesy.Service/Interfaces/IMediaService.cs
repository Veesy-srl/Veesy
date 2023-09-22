using Veesy.Domain.Exception;
using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface IMediaService
{
    List<Media> GetAllByUserId(MyUser user);
    Task AddMedia(Media media, MyUser user);
    Media? GetMediaById(Guid id);
    Media? GetPreviousMediaByDate(DateTime mediaSelectedCreateRecordDate, MyUser user);
    Media? GetNextMediaByDate(DateTime mediaSelectedCreateRecordDate, MyUser user);
}