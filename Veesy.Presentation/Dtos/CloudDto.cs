using Veesy.Domain.Models;

namespace Veesy.Service.Dtos;

public class MediaDto
{
    public Guid Code { get; set; }
    public string OriginalFileName { get; set; }
    public string FileName { get; set; }
    public long Size { get; set; }
    public string Type { get; set; }
    public string UploadDate { get; set; }
    public string? Credits { get; set; }
}

public static class MapCloudDtos
{
    public static List<MediaDto> MapMediaList(List<Domain.Models.Media> media)
    {
        return media.Select(x => new MediaDto()
        {
            Code = x.Id,
            OriginalFileName = x.OriginalFileName,
            FileName = x.FileName,
            Size = x.Size,
            Type = x.Type,
            Credits = x.Credits,
            UploadDate = x.CreateRecordDate.ToString("dd/MM/yyyy hh.mm")
        }).ToList();
    }
    
    public static MediaDto MapMedia(Media? media)
    {
        if (media == null)
            return null;
        
        return new MediaDto()
        {
            Code = media.Id,
            OriginalFileName = media.OriginalFileName,
            FileName = media.FileName,
            Size = media.Size,
            Type = media.Type,
            Credits = media.Credits,
            UploadDate = media.CreateRecordDate.ToString("dd/MM/yyyy hh.mm")
        };
    }
}