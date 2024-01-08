using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Cloud;

public class CloudViewModel
{
    public List<MediaDto> Medias { get; set; }
    public string Username { get; set; }
    public string BasePath { get; set; }
    public string ApplicationUrl { get; set; }
    public string BasePathAzure { get; set; }
    public List<PortfolioThumbnailDto> Portfolios { get; set; }
}