using Veesy.Domain.Models;

namespace Veesy.Service.Dtos;

public class MediaDto
{
    public Guid Code { get; set; }
    public string OriginalFileName { get; set; }
    public string FileName { get; set; }
    public string Size { get; set; }
    public string Type { get; set; }
    public string UploadDate { get; set; }
    public string Credits { get; set; }
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
            Size = x.Size.ToString(),
            Type = x.Type,
            UploadDate = x.CreateRecordDate.ToString("dd/MM/yyyy h. hh.mm")
        }).ToList();
    }
    
    public static MediaDto MapMedia(Domain.Models.Media media, string basePath)
    {
        return new MediaDto()
        {
            Code = media.Id,
            OriginalFileName = media.OriginalFileName,
            FileName = media.FileName,
            Size = media.Size.ToString(),
            Type = media.Type,
            UploadDate = media.CreateRecordDate.ToString("dd/MM/yyyy h. hh.mm")
        };
    }
}