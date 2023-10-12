using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Presentation.Model.Cloud;
using Veesy.Service.Dtos;
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