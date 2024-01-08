using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Portfolio;

public class PortfolioListViewModel
{
    public List<PortfolioThumbnailDto> PortfolioThumbnailDtos { get; set; }
    public string BasePath { get; set; }
    public string ApplicationUrl { get; set; }
}