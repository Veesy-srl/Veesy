using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Portfolio;

public class PortfolioViewModel
{
    public PreviewPortfolioDto PortfolioDto { get; set; }
    public string ApplicationUrl { get; set; }
    public string BasePathImages { get; set; }
    public string BasePathAzure { get; set; }
    public bool IsPublish { get; set; }
    public bool Unlocked { get; set; }
    public string Password { get; set; }
    public int ControlPassword { get; set; }
    public bool OpenPopup { get; set; }
}