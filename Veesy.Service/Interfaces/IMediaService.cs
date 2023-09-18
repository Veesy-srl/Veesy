using Veesy.Domain.Models;

namespace Veesy.Service.Interfaces;

public interface IMediaService
{
    List<Media> GetAllByUserId(MyUser user);
}