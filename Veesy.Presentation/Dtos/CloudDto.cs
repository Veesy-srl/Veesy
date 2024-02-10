using Veesy.Domain.Models;
using Veesy.Domain.Constants;
    
namespace Veesy.Service.Dtos;

public class MediaDto
{
    public Guid Code { get; set; }
    public string OriginalFileName { get; set; }
    public string FileName { get; set; }
    public Guid? NestedPortfolioLinks { get; set; }
    public long Size { get; set; }
    public string Type { get; set; }
    public string UploadDate { get; set; }
    public string? Credits { get; set; }
    public OwnerDto? Owner { get; set; }
    public bool? IsVideo => !string.IsNullOrEmpty(Type) ? MediaCostants.VideoExtensions.Contains(Type.ToUpper()) : null;
}

public class MediaGalleryDto: MediaDto
{
    public string PortfolioCode { get; set; }
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

public class OwnerDto{
    public string Email { get; set; }
    public string ProfileImageFileName { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Fullname => Name + " " + Surname;
    public string Category { get; set; }
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

    public static OwnerDto MapOwnerDto(MyUser user)
    {
        return new OwnerDto()
        {
            ProfileImageFileName = user.ProfileImageFileName,
            Email = user.Email,
            Category = user.Category,
            Name = user.Name,
            Surname = user.Surname
        };
    }
    
    public static List<MediaDto> MapMediaList(List<Media> media)
    {
        return media.Select(x => new MediaDto()
        {
            Code = x.Id,
            OriginalFileName = x.OriginalFileName,
            FileName = x.FileName,
            NestedPortfolioLinks = x.NestedPortfolioLinks,
            Size = x.Size,
            Type = x.Type,
            Credits = x.Credits,
            UploadDate = x.CreateRecordDate.ToString("dd/MM/yyyy hh.mm"),
            Owner = x.MyUser != null ? MapOwnerDto(x.MyUser) : null
        }).ToList();
    }
    
    public static List<MediaGalleryDto> MapMediaGalleryList(List<Media> media)
    {
        return media.Select(x => new MediaGalleryDto()
        {
            Code = x.Id,
            OriginalFileName = x.OriginalFileName,
            FileName = x.FileName,
            NestedPortfolioLinks = x.NestedPortfolioLinks,
            Size = x.Size,
            Type = x.Type,
            Credits = x.Credits,
            UploadDate = x.CreateRecordDate.ToString("dd/MM/yyyy hh.mm"),
            PortfolioCode = x.PortfolioMedias[0].Portfolio.Id.ToString(),
            Owner = x.MyUser != null ? MapOwnerDto(x.MyUser) : null
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
            NestedPortfolioLinks = media.NestedPortfolioLinks,
            Size = media.Size,
            Type = media.Type,
            Credits = media.Credits,
            UploadDate = media.CreateRecordDate.ToString("dd/MM/yyyy hh.mm"),
            Owner = media.MyUser != null ? MapOwnerDto(media.MyUser) : null
        };
    }
}