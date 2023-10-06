using Veesy.Domain.Constants;
using Veesy.Domain.Models;

namespace Veesy.Service.Dtos;

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
    public DateTime LastEditRecordDate { get; set; }
    public bool IsMain { get; set; }
    public VeesyConstants.PortfolioLayout Layout { get; set; }
    public virtual List<PortfolioMediaDto> PortfolioMedias { get; set; }

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

public class PortfolioThumbnailDto
{
    public Guid Code { get; set; }
    public string Name { get; set; }
    public int NumberMedia { get; set; }
    public string LastUpdate { get; set; }
    public string DefaultImageName { get; set; }
    public string DefaultImageOriginalName { get; set; }
    public bool IsMain { get; set; }
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
            DefaultImageName = portfolio.PortfolioMedias.Count == 0
                ? ""
                : portfolio.PortfolioMedias.SingleOrDefault(s => s.SortOrder == 0).Media.FileName,
            DefaultImageOriginalName = portfolio.PortfolioMedias.Count == 0
                ? ""
                : portfolio.PortfolioMedias.SingleOrDefault(s => s.SortOrder == 0).Media.OriginalFileName,
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
            DefaultImageName = portfolio.PortfolioMedias.Count == 0
                ? ""
                : portfolio.PortfolioMedias.SingleOrDefault(s => s.SortOrder == 0).Media.FileName,
            DefaultImageOriginalName = portfolio.PortfolioMedias.Count == 0
                ? ""
                : portfolio.PortfolioMedias.SingleOrDefault(s => s.SortOrder == 0).Media.OriginalFileName,
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
            LastEditRecordDate = portfolio.LastEditRecordDate,
            IsMain = portfolio.IsMain,
            Layout = portfolio.Layout,
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
            SortOrder = portfolioMedia.SortOrder
        };
    }
}
