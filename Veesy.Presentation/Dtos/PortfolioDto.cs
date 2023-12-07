using Veesy.Domain.Constants;
using Veesy.Domain.Models;

namespace Veesy.Service.Dtos;

public class PreviewPortfolioDto
{
    public Guid Code { get; set; }
    public string MyUserId { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; }
    public string UserDescription { get; set; }
    public string UserFullName { get; set; }
    public string UserCategory { get; set; }
    public string PortfolioIntro { get; set; }
    public VeesyConstants.PortfolioLayout Layout { get; set; }
    public virtual List<PortfolioMediaDto> PortfolioMedias { get; set; }
    public string UserEmail { get; set; }
    public string UserImage { get; set; }
    public bool ShowSoftSkill { get; set; }
    public bool ShowFields { get; set; }
    public bool ShowCV { get; set; }
    public bool IsPublic { get; set; }
    public bool ShowSoftware { get; set; }
    public List<string> UsedSoftwares { get; set; }
    public List<string> SoftSkill { get; set; }
    public List<string> Sector { get; set; }
    public List<string> LanguageSpoken { get; set; }
}

public class PortfolioDto
{
    public Guid Id { get; set; }
    public string MyUserId { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
    public bool IsPublic { get; set; }
    public string Password { get; set; }
    public string Link { get; set; }
    public int Status { get; set; }
    public string LastEditRecordDate { get; set; }
    public bool IsMain { get; set; }
    public int NumberImage { get; set; }
    public int NumberVideo { get; set; }
    public MediaDto DefaultMedia { get; set; }
    public int NumberMedia => NumberImage + NumberVideo;
    public VeesyConstants.PortfolioLayout Layout { get; set; }
    public virtual List<PortfolioMediaDto>? PortfolioMedias { get; set; }

    public PortfolioDto()
    {
        PortfolioMedias = new List<PortfolioMediaDto>();
    }
}

public class NewPortfolioDto
{
    public List<Guid> CodeImagesToAdd { get; set; }
    public string Name { get; set; }
    
}

public class EditPortfolioDto
{
    public List<Guid> CodeImagesToAdd { get; set; }
    public Guid Code { get; set; }
}

public class UpdatePortfolioDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public bool IsPublic { get; set; }
    public bool IsMain { get; set; }
    public int LayoutGrid { get; set; }
}

public class UpdateMediaPortfolioDto
{
    public List<Guid> PortfolioSelected { get; set; }
    public Guid MediaCode { get; set; }
}

public class UpdateMediaSortOrderDto
{
    public Guid PortfolioId { get; set; }
    public UpdateMediaSortOrderCoupleDto[] NewMediasSortOrder { get; set; }
}

public class UpdateMediaSortOrderCoupleDto
{
    public Guid MediaId { get; set; }
    public int SortOrder { get; set; }
}

public class PortfolioThumbnailDto
{
    public Guid Code { get; set; }
    public string Name { get; set; }
    public int NumberMedia { get; set; }
    public string LastUpdate { get; set; }
    public string? DefaultImageName { get; set; }
    public string? DefaultImageOriginalName { get; set; }
    public bool IsMain { get; set; }
    public bool IsVideo { get; set; }
}

public class PortfolioMediaDto
{
    public Guid MediaId { get; set; }
    public MediaDto Media { get; set; }
    public Guid PortfolioId { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
}

public static class MapPortfolioDtos
{
    public static PortfolioThumbnailDto MapPortfolioThumbnailDto(Portfolio? portfolio)
    {
        if (portfolio == null)
            return null;

        return new PortfolioThumbnailDto()
        {
            Code = portfolio.Id,
            IsMain = portfolio.IsMain,
            NumberMedia = portfolio.PortfolioMedias.Count,
            Name = portfolio.Name,
            IsVideo = MediaCostants.VideoExtensions.Contains(portfolio.PortfolioMedias
                .SingleOrDefault(s => s.SortOrder == 0)
                ?.Media.Type.ToUpper()),
            DefaultImageName = portfolio.PortfolioMedias.Count == 0
                ? ""
                : portfolio.PortfolioMedias.SingleOrDefault(s => s.SortOrder == 0)?.Media.FileName,
            DefaultImageOriginalName = portfolio.PortfolioMedias.Count == 0
                ? ""
                : portfolio.PortfolioMedias.SingleOrDefault(s => s.SortOrder == 0)?.Media.OriginalFileName,
            LastUpdate = portfolio.LastEditRecordDate.ToString("dd.MM.yyyy")
        };
    }

    public static List<PortfolioThumbnailDto> MapListPortfolioThumbnailDto(List<Portfolio>? portfolios)
    {
        if (portfolios == null)
            return null;

        var portfoliosDto = new List<PortfolioThumbnailDto>();

        portfolios.ForEach(portfolio => portfoliosDto.Add(new PortfolioThumbnailDto()
        {
            Code = portfolio.Id,
            IsMain = portfolio.IsMain,
            NumberMedia = portfolio.PortfolioMedias.Count,
            Name = portfolio.Name,
            IsVideo = portfolio.PortfolioMedias.Count != 0 && MediaCostants.VideoExtensions.Contains(portfolio.PortfolioMedias
                .SingleOrDefault(s => s.SortOrder == 0)
                ?.Media.Type.ToUpper()),
            DefaultImageName = portfolio.PortfolioMedias.Count == 0
                ? ""
                : portfolio.PortfolioMedias.SingleOrDefault(s => s.SortOrder == 0)?.Media.FileName,
            DefaultImageOriginalName = portfolio.PortfolioMedias.Count == 0
                ? ""
                : portfolio.PortfolioMedias.SingleOrDefault(s => s.SortOrder == 0)?.Media.OriginalFileName,
            LastUpdate = portfolio.LastEditRecordDate.ToString("dd.MM.yyyy")
        }));
        return portfoliosDto;
    }

    public static PortfolioDto MapPortfolio(Portfolio? portfolio)
    {
        if (portfolio == null)
            return null;

        return new PortfolioDto()
        {
            Id = portfolio.Id,
            MyUserId = portfolio.MyUserId,
            Name = portfolio.Name,
            Description = portfolio.Description,
            Note = portfolio.Note,
            IsPublic = portfolio.IsPublic,
            Password = portfolio.Password,
            Link = portfolio.Link,
            Status = portfolio.Status,
            LastEditRecordDate = portfolio.LastEditRecordDate.ToString("dd.MM.yyyy"),
            IsMain = portfolio.IsMain,
            Layout = portfolio.Layout,
            NumberImage = portfolio.PortfolioMedias.Count == 0
                ? 0
                : portfolio.PortfolioMedias.Count(s => MediaCostants.ImageExtensions.Contains(s.Media.Type.ToUpper())),
            NumberVideo = portfolio.PortfolioMedias.Count == 0
                ? 0
                : portfolio.PortfolioMedias.Count(s => MediaCostants.VideoExtensions.Contains(s.Media.Type.ToUpper())),
            DefaultMedia = portfolio.PortfolioMedias.Count == 0
                ? null
                : MapCloudDtos.MapMedia(portfolio.PortfolioMedias.SingleOrDefault(s => s.SortOrder == 0)?.Media),
            PortfolioMedias = portfolio.PortfolioMedias?.Select(MapPortfolioMedia)?.ToList()
        };
    }

    public static PortfolioMediaDto MapPortfolioMedia(PortfolioMedia portfolioMedia)
    {
        if (portfolioMedia == null)
            return null;

        return new PortfolioMediaDto()
        {
            MediaId = portfolioMedia.MediaId,
            PortfolioId = portfolioMedia.PortfolioId,
            Description = portfolioMedia.Description,
            IsActive = portfolioMedia.IsActive,
            SortOrder = portfolioMedia.SortOrder,
            Media = MapCloudDtos.MapMedia(portfolioMedia.Media)
        };
    }

    public static List<PortfolioMediaDto> MapListPortfolioMedia(List<PortfolioMedia> portfolioMedias)
    {
        if (portfolioMedias == null)
            return null;

        var list = new List<PortfolioMediaDto>();
        foreach (var portfolioMedia in portfolioMedias)
        {
            list.Add(new PortfolioMediaDto()
            {
                MediaId = portfolioMedia.MediaId,
                PortfolioId = portfolioMedia.PortfolioId,
                Description = portfolioMedia.Description,
                IsActive = portfolioMedia.IsActive,
                SortOrder = portfolioMedia.SortOrder,
                Media = MapCloudDtos.MapMedia(portfolioMedia.Media)
            });
        }

        return list;
    }

    public static PreviewPortfolioDto MapPreviewPortfolioDto(Portfolio? portfolio, List<string> languageSpoken,
        List<string> sector, List<string> usedSoftware, List<string> softSkill, List<MyUserInfoToShow> infoToShow)
    {
        if (portfolio == null)
            return null;

        return new PreviewPortfolioDto()
        { 
            Code = portfolio.Id,
            Description = portfolio.Description,
            PortfolioMedias = MapListPortfolioMedia(portfolio.PortfolioMedias),
            Layout = portfolio.Layout,
            UserDescription = portfolio.MyUser.Biografy,
            UserFullName = portfolio.MyUser.Fullname,
            MyUserId = portfolio.MyUserId,
            Name = portfolio.Name,
            UserCategory = portfolio.MyUser.Category,
            PortfolioIntro = portfolio.MyUser.PortfolioIntro,
            UserEmail = portfolio.MyUser.Email,
            IsPublic = portfolio.IsPublic,
            UserImage = portfolio.MyUser.ProfileImageFileName,
            Sector = sector,
            UsedSoftwares = usedSoftware,
            SoftSkill = softSkill,
            LanguageSpoken = languageSpoken,
            ShowSoftSkill = infoToShow.SingleOrDefault(s => s.InfoToShow.Info == VeesyConstants.InfoToShow.SoftSkill) !=
                            null,
            ShowFields = infoToShow.SingleOrDefault(s => s.InfoToShow.Info == VeesyConstants.InfoToShow.Fields) != null,
            ShowCV = infoToShow.SingleOrDefault(s => s.InfoToShow.Info == VeesyConstants.InfoToShow.CV) != null,
            ShowSoftware = infoToShow.SingleOrDefault(s => s.InfoToShow.Info == VeesyConstants.InfoToShow.Software) !=
                           null,
        };
    }
}

