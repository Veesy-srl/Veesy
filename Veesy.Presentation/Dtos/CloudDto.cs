using Veesy.Domain.Models;
using Veesy.Domain.Constants;
    
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
    public bool? IsVideo => !string.IsNullOrEmpty(Type) ? MediaCostants.VideoExtensions.Contains(Type.ToUpper()) : null;
}

public class UploadMediaResponseDto
{
    public UploadMediaResponseDto()
    {
        MediaDtos = new List<MediaDto>();
    }

    public int NumberSuccessFile { get; set; }
    public int NumberErrorFile { get; set; }
    public string SuccessFileMessage { get; set; }
    public string ErrorFileMessage { get; set; }
    public List<MediaDto> MediaDtos { get; set; }
}

public class LinkPortfolioDto{
    public Guid Code { get; set; }
    public string Name { get; set; }
    public bool Selected { get; set; }
}

public static class MapCloudDtos
{

    public static List<LinkPortfolioDto> MapLinkedPortfolioDtos(List<Portfolio> portfolios, Media media)
    {
        var portfoliosDto = new List<LinkPortfolioDto>();
        portfolios.ForEach(portfolio => portfoliosDto.Add(new LinkPortfolioDto()
        {
            Code = portfolio.Id,
            Name = portfolio.Name,
            Selected = portfolio.PortfolioMedias.Select(s => s.MediaId).Contains(media.Id)
        }));
        return portfoliosDto;
    }
    
    public static List<MediaDto> MapMediaList(List<Media> media)
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