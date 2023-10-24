using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Portfolio;

public class PortfolioViewModel
{
    public PreviewPortfolioDto PortfolioDto { get; set; }
    public string BasePathImages { get; set; }
    public string BasePathAzure { get; set; }
}