using Veesy.Domain.Models;
using Veesy.Service.Interfaces;

namespace Veesy.Presentation.Helper;

public class PublicHelper
{
    private readonly IMediaService _mediaService;

    public PublicHelper(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    public List<(string ImgPath, string userImg, string Username)> GetListMedia(int count)
    {
        var mediaListWithUsernames = new List<(string, string, string)>();
       
        mediaListWithUsernames = _mediaService.GetRandomMediaWithUsername(count);

        return mediaListWithUsernames;
    }
}