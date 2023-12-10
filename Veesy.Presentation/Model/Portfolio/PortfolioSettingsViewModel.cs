using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Portfolio;

public class PortfolioSettingsViewModel
{
    public PortfolioDto Portfolio { get; set; }
    public string BasePathImages { get; set; }
    public string ApplicationUrl { get; set; }
}