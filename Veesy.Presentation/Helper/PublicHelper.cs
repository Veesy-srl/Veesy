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

    public List<(Media Media, string userImg, string Username)> GetListMedia(int count)
    {
        var mediaListWithUsernames = new List<(Media, string, string)>();
        var i = 0;
        while (i < count)
        {
            var randomMediaWithUser = _mediaService.GetRandomMediaWithUsername();
            if (randomMediaWithUser.Item1 != null && randomMediaWithUser.Item2 != null)
            {
                mediaListWithUsernames.Add(randomMediaWithUser);
                i++;
            }
        }

        return mediaListWithUsernames;
    }
}